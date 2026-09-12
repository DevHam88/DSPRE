using DSPRE.ROMFiles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DSPRE.Editors
{
    public partial class TrainerClassPresentationEditor : Form
    {
        private readonly int classId;
        private readonly TrainerClassMetadataRecord record;
        private readonly List<string> trainerNames;
        private readonly Dictionary<TrainerClassMetadataAssetField, DataGridViewCell> assetCells =
            new Dictionary<TrainerClassMetadataAssetField, DataGridViewCell>();
        private bool loading;
        private bool dirty;

        public TrainerClassPresentationEditor(int classId, string classLabel,
            TrainerClassMetadataRecord record)
        {
            InitializeComponent();
            this.classId = classId;
            this.record = record ?? throw new ArgumentNullException(nameof(record));
            trainerNames = new TextArchive(RomInfo.trainerNamesMessageNumber).GetSimpleTrainerNames();

            loading = true;
            trainerClassLabel.Text = classLabel;
            ConfigureStyleSelector();
            ConfigureNameSelector();
            ConfigureAssetGrid();
            LoadRecord();
            loading = false;
            dirty = false;
            UpdateApplicability();
        }

        private void ConfigureStyleSelector()
        {
            styleComboBox.Items.AddRange(TrainerClassMetadataSchema.StyleNames.Cast<object>().ToArray());
        }

        private void ConfigureNameSelector()
        {
            for (int i = 0; i < trainerNames.Count; i++)
            {
                trainerNameComboBox.Items.Add("[" + i.ToString("D4") + "] " + trainerNames[i]);
            }
        }

        private void ConfigureAssetGrid()
        {
            assetsGrid.Columns.Clear();
            string[] headers = { "RLCN", "RGCN", "RECN", "RNAN", "RCSN 1", "RCSN 2", "RCSN 3" };
            foreach (string header in headers)
            {
                assetsGrid.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = header,
                    Name = header.Replace(" ", string.Empty),
                    SortMode = DataGridViewColumnSortMode.NotSortable,
                    Width = 82
                });
            }

            assetsGrid.Rows.Add(3);
            string[] rowLabels = { "Group 1", "Group 2", "Group 3" };
            for (int row = 0; row < rowLabels.Length; row++)
            {
                assetsGrid.Rows[row].HeaderCell.Value = rowLabels[row];
                for (int column = 0; column < assetsGrid.Columns.Count; column++)
                {
                    if (!TryGetAssetField(row, column, out TrainerClassMetadataAssetField field))
                    {
                        DataGridViewCell cell = assetsGrid.Rows[row].Cells[column];
                        cell.ReadOnly = true;
                        cell.Value = null;
                        cell.Style.BackColor = SystemColors.ControlDark;
                        cell.Style.SelectionBackColor = SystemColors.ControlDark;
                        continue;
                    }

                    DataGridViewCell assetCell = assetsGrid.Rows[row].Cells[column];
                    assetCell.Value = record.GetAsset(field);
                    assetCells[field] = assetCell;
                }
            }
        }

        private static bool TryGetAssetField(int row, int column,
            out TrainerClassMetadataAssetField field)
        {
            field = default;
            if (row < 0 || row > 2 || column < 0 || column > 6 || (row > 0 && column > 3))
                return false;

            int offset = row == 0 ? 0x16 + column * 2 : row == 1 ? 0x24 + column * 2 : 0x2C + column * 2;
            field = (TrainerClassMetadataAssetField)offset;
            return true;
        }

        private void LoadRecord()
        {
            styleComboBox.SelectedIndex = record.VsStyle <= TrainerClassMetadataSchema.MaximumStyle
                ? record.VsStyle
                : -1;

            if (record.TrainerNameId < trainerNameComboBox.Items.Count)
                trainerNameComboBox.SelectedIndex = record.TrainerNameId;
            else
                trainerNameComboBox.Text = "[" + record.TrainerNameId + "] <missing>";

            savedRivalComboBox.SelectedIndex = record.UseSavedRivalName <= 1
                ? record.UseSavedRivalName
                : -1;
            style1MotionUpDown.Value = record.Style1Motion;
            style2TimingUpDown.Value = record.Style2Timing;
        }

        private void UpdateApplicability()
        {
            ushort style = styleComboBox.SelectedIndex >= 0
                ? (ushort)styleComboBox.SelectedIndex
                : ushort.MaxValue;
            bool isStyle1 = style == 1;
            bool usesSavedRival = isStyle1 && savedRivalComboBox.SelectedIndex == 1;
            bool usesStaticName = style == 2 || style == 3 || style == 13 || (isStyle1 && !usesSavedRival);

            trainerNameLabel.Enabled = trainerNameComboBox.Enabled = usesStaticName;
            savedRivalLabel.Enabled = savedRivalComboBox.Enabled = isStyle1;
            style1MotionLabel.Enabled = style1MotionUpDown.Enabled = isStyle1;
            style2TimingLabel.Enabled = style2TimingUpDown.Enabled = style == 2;

            foreach (KeyValuePair<TrainerClassMetadataAssetField, DataGridViewCell> pair in assetCells)
            {
                bool consumed = TrainerClassMetadataSchema.IsAssetConsumed(style, pair.Key);
                pair.Value.Style.BackColor = consumed ? SystemColors.Window : SystemColors.Control;
                pair.Value.Style.ForeColor = consumed ? SystemColors.WindowText : SystemColors.GrayText;
                pair.Value.Style.SelectionBackColor = consumed ? SystemColors.Highlight : SystemColors.ControlDark;
                pair.Value.Style.SelectionForeColor = consumed ? SystemColors.HighlightText : SystemColors.ControlText;
            }
        }

        private void MarkDirty(object sender, EventArgs e)
        {
            if (!loading) dirty = true;
        }

        private void styleComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateApplicability();
            MarkDirty(sender, e);
        }

        private void savedRivalComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateApplicability();
            MarkDirty(sender, e);
        }

        private void assetsGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!loading && e.RowIndex >= 0) dirty = true;
        }

        private void assetsGrid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (!TryGetAssetField(e.RowIndex, e.ColumnIndex, out _)) return;

            string value = Convert.ToString(e.FormattedValue)?.Trim();
            if (!ushort.TryParse(value, out _))
            {
                assetsGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText =
                    "Enter an integer from 0 to 65535.";
                e.Cancel = true;
            }
            else
            {
                assetsGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = string.Empty;
            }
        }

        private bool TryApplyControls(out string error)
        {
            error = null;
            if (!assetsGrid.EndEdit())
            {
                error = "Finish or correct the current asset-member edit before saving.";
                return false;
            }
            if (styleComboBox.SelectedIndex < 0)
            {
                error = "Select a VS style in the range 0..13.";
                return false;
            }

            record.VsStyle = (ushort)styleComboBox.SelectedIndex;
            if (savedRivalComboBox.SelectedIndex >= 0)
                record.UseSavedRivalName = (byte)savedRivalComboBox.SelectedIndex;
            if (trainerNameComboBox.SelectedIndex >= 0)
                record.TrainerNameId = (ushort)trainerNameComboBox.SelectedIndex;
            record.Style1Motion = decimal.ToUInt32(style1MotionUpDown.Value);
            record.Style2Timing = decimal.ToUInt16(style2TimingUpDown.Value);

            foreach (KeyValuePair<TrainerClassMetadataAssetField, DataGridViewCell> pair in assetCells)
            {
                if (!ushort.TryParse(Convert.ToString(pair.Value.Value), out ushort memberId))
                {
                    error = TrainerClassMetadataSchema.GetAssetLabel(pair.Key) +
                        " must be an integer from 0 to 65535.";
                    return false;
                }
                record.SetAsset(pair.Key, memberId);
            }
            return true;
        }

        private bool TrySave()
        {
            if (!TryApplyControls(out string error))
            {
                MessageBox.Show(error, "Invalid presentation metadata", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            TrainerClassMetadataValidationResult validation =
                TrainerClassMetadataStore.ValidatePresentation(record, trainerNames.Count);
            if (!validation.IsValid)
            {
                MessageBox.Show(string.Join(Environment.NewLine, validation.Errors),
                    "Invalid presentation metadata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (validation.Warnings.Count > 0)
            {
                DialogResult warningResult = MessageBox.Show(
                    string.Join(Environment.NewLine, validation.Warnings) +
                    "\n\nSave these values anyway?",
                    "Unexpected asset types", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (warningResult != DialogResult.Yes) return false;
            }

            if (!TrainerClassMetadataStore.TryWritePresentationFields(classId, record, out error))
            {
                MessageBox.Show(error, "Trainer-class metadata not saved", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            dirty = false;
            return true;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (!TrySave()) return;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void TrainerClassPresentationEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!dirty || DialogResult == DialogResult.OK) return;

            DialogResult result = MessageBox.Show("Save changes to this presentation record?",
                "Unsaved Changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            if (result == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
            else if (result == DialogResult.Yes)
            {
                if (!TrySave())
                {
                    e.Cancel = true;
                    return;
                }
                DialogResult = DialogResult.OK;
            }
        }
    }
}
