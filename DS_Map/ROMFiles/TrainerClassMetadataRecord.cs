using System;
using System.Collections.Generic;
using System.IO;
using static DSPRE.RomInfo;

namespace DSPRE.ROMFiles
{
    public enum TrainerClassMetadataDetectionState
    {
        Stock,
        SchemaV1,
        Inconsistent
    }

    public sealed class TrainerClassMetadataCommonFields
    {
        public ushort Gender { get; set; }
        public ushort PrizeCoefficient { get; set; }
        public ushort MainEyeContactMusic { get; set; }
        public ushort AlternateEyeContactMusic { get; set; }
    }

    public static class TrainerClassMetadataStore
    {
        public const int RecordLength = 0x34;
        public const int MinimumRecordCount = 129;

        private const uint GenderHookOffset = 0x735F8;
        private const uint EyeContactHookOffset = 0x55098;
        private const int ExpectedSpeciesEntryCount = 43;

        private static string detectedWorkDir;
        private static TrainerClassMetadataDetectionState detectionState;
        private static string detectionDetail;
        private static int recordCount;

        public static int RecordCount => recordCount;

        public static void Reset()
        {
            detectedWorkDir = null;
            detectionState = TrainerClassMetadataDetectionState.Stock;
            detectionDetail = null;
            recordCount = 0;
        }

        public static TrainerClassMetadataDetectionState DetectCurrentRom(out string detail)
        {
            string currentWorkDir = RomInfo.workDir ?? string.Empty;
            if (detectedWorkDir == currentWorkDir && detectionDetail != null)
            {
                detail = detectionDetail;
                return detectionState;
            }

            detectedWorkDir = currentWorkDir;
            recordCount = 0;

            if (RomInfo.gameFamily != GameFamilies.HGSS || RomInfo.gameLanguage != GameLanguages.English)
            {
                detectionState = TrainerClassMetadataDetectionState.Stock;
                detectionDetail = "TCM schema-v1 detection is limited to English HeartGold and SoulSilver.";
                detail = detectionDetail;
                return detectionState;
            }

            bool archiveMarker = TryInspectArchive(out recordCount, out string archiveDetail);
            bool genderHookMarker = HasThumbVeneer(GenderHookOffset, 0x00, 0x49, 0x08, 0x47);
            bool eyeContactHookMarker = HasThumbVeneer(EyeContactHookOffset, 0x00, 0x4A, 0x10, 0x47);

            RomInfo.SetBattleEffectsData();
            bool retiredTrainerTableMarker = TryReadByte(RomInfo.vsTrainerEntryTableOffsetToSizeLimiter, out byte trainerCount) &&
                TryReadUInt32(RomInfo.vsTrainerEntryTableOffsetToRAMAddress, out uint trainerPointer) &&
                trainerCount == 0 && trainerPointer == 0;
            bool speciesTableMarker = TryReadByte(RomInfo.vsPokemonEntryTableOffsetToSizeLimiter, out byte speciesCount) &&
                speciesCount >= ExpectedSpeciesEntryCount;

            bool[] markers =
            {
                archiveMarker,
                genderHookMarker,
                eyeContactHookMarker,
                retiredTrainerTableMarker,
                speciesTableMarker
            };

            bool anyMarker = false;
            bool allMarkers = true;
            foreach (bool marker in markers)
            {
                anyMarker |= marker;
                allMarkers &= marker;
            }

            if (allMarkers)
            {
                detectionState = TrainerClassMetadataDetectionState.SchemaV1;
                detectionDetail = "TCM schema v1-compatible data detected.";
            }
            else if (!anyMarker)
            {
                detectionState = TrainerClassMetadataDetectionState.Stock;
                detectionDetail = "No TCM schema-v1 markers detected.";
            }
            else
            {
                detectionState = TrainerClassMetadataDetectionState.Inconsistent;
                var failedMarkers = new List<string>();
                if (!archiveMarker) failedMarkers.Add("a155 structure (" + archiveDetail + ")");
                if (!genderHookMarker) failedMarkers.Add("gender hook");
                if (!eyeContactHookMarker) failedMarkers.Add("eye-contact hook");
                if (!retiredTrainerTableMarker) failedMarkers.Add("retired trainer-to-combo table");
                if (!speciesTableMarker) failedMarkers.Add("species-to-combo table with at least 43 rows");
                detectionDetail = "Partial TCM installation: missing or invalid " + string.Join(", ", failedMarkers) + ".";
            }

            detail = detectionDetail;
            return detectionState;
        }

        public static bool EnsureUnpacked(out string error)
        {
            error = null;
            TrainerClassMetadataDetectionState state = DetectCurrentRom(out string detail);
            if (state != TrainerClassMetadataDetectionState.SchemaV1)
            {
                error = detail;
                return false;
            }

            try
            {
                DSUtils.TryUnpackNarcs(new List<DirNames> { DirNames.trainerClassMetadata });
                string unpackedDir = RomInfo.gameDirs[DirNames.trainerClassMetadata].unpackedDir;
                if (!Directory.Exists(unpackedDir))
                {
                    error = "The trainer-class metadata archive could not be unpacked.";
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        public static bool TryReadCommonFields(int classId, out TrainerClassMetadataCommonFields fields, out string error)
        {
            fields = null;
            if (!TryReadRecord(classId, out byte[] record, out error))
            {
                return false;
            }

            ushort gender = BitConverter.ToUInt16(record, 0x00);
            if (gender > 1)
            {
                error = "Trainer-class metadata gender must be 0 (male or multi) or 1 (female).";
                return false;
            }

            fields = new TrainerClassMetadataCommonFields
            {
                Gender = gender,
                PrizeCoefficient = BitConverter.ToUInt16(record, 0x02),
                MainEyeContactMusic = BitConverter.ToUInt16(record, 0x04),
                AlternateEyeContactMusic = BitConverter.ToUInt16(record, 0x06)
            };
            return true;
        }

        public static bool TryWriteCommonFields(int classId, TrainerClassMetadataCommonFields fields, out string error)
        {
            error = null;
            if (fields == null)
            {
                error = "No trainer-class metadata values were supplied.";
                return false;
            }
            if (fields.Gender > 1)
            {
                error = "Trainer-class metadata gender must be 0 (male or multi) or 1 (female).";
                return false;
            }
            if (!TryReadRecord(classId, out byte[] record, out error))
            {
                return false;
            }

            Buffer.BlockCopy(BitConverter.GetBytes(fields.Gender), 0, record, 0x00, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(fields.PrizeCoefficient), 0, record, 0x02, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(fields.MainEyeContactMusic), 0, record, 0x04, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(fields.AlternateEyeContactMusic), 0, record, 0x06, 2);

            try
            {
                File.WriteAllBytes(GetRecordPath(classId), record);
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        private static bool TryReadRecord(int classId, out byte[] record, out string error)
        {
            record = null;
            error = null;
            if (classId < 0)
            {
                error = "Trainer-class index cannot be negative.";
                return false;
            }
            if (!EnsureUnpacked(out error))
            {
                return false;
            }
            if (classId >= recordCount)
            {
                error = "Trainer-class metadata record " + classId + " does not exist.";
                return false;
            }

            string path = GetRecordPath(classId);
            if (!File.Exists(path))
            {
                error = "Trainer-class metadata record is missing: " + path;
                return false;
            }

            try
            {
                record = File.ReadAllBytes(path);
                if (record.Length != RecordLength)
                {
                    error = "Trainer-class metadata record " + classId + " is " + record.Length +
                        " bytes; expected " + RecordLength + ".";
                    record = null;
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        private static string GetRecordPath(int classId)
        {
            return Path.Combine(RomInfo.gameDirs[DirNames.trainerClassMetadata].unpackedDir, classId.ToString("D4"));
        }

        private static bool TryInspectArchive(out int memberCount, out string detail)
        {
            memberCount = 0;
            detail = "archive is missing";
            if (!RomInfo.gameDirs.TryGetValue(DirNames.trainerClassMetadata, out (string packedDir, string unpackedDir) paths) ||
                !File.Exists(paths.packedDir))
            {
                return false;
            }

            try
            {
                using (var reader = new BinaryReader(File.OpenRead(paths.packedDir)))
                {
                    long fileLength = reader.BaseStream.Length;
                    if (fileLength < 0x34 || reader.ReadUInt32() != 0x4352414E)
                    {
                        detail = "invalid NARC header";
                        return false;
                    }

                    reader.BaseStream.Position = 0x08;
                    uint declaredLength = reader.ReadUInt32();
                    ushort headerLength = reader.ReadUInt16();
                    ushort sectionCount = reader.ReadUInt16();
                    if (declaredLength != fileLength || headerLength != 0x10 || sectionCount != 3)
                    {
                        detail = "invalid NARC size or section header";
                        return false;
                    }

                    reader.BaseStream.Position = 0x10;
                    if (reader.ReadUInt32() != 0x46415442)
                    {
                        detail = "missing FATB section";
                        return false;
                    }
                    uint fatbLength = reader.ReadUInt32();
                    uint count = reader.ReadUInt32();
                    if (count < MinimumRecordCount || count > int.MaxValue || fatbLength != 12 + count * 8)
                    {
                        detail = "unexpected member count or FATB size";
                        return false;
                    }

                    long fatEntriesOffset = 0x1C;
                    long fntbOffset = 0x10 + fatbLength;
                    if (fntbOffset + 8 > fileLength)
                    {
                        detail = "truncated FNTB section";
                        return false;
                    }
                    reader.BaseStream.Position = fntbOffset;
                    if (reader.ReadUInt32() != 0x464E5442)
                    {
                        detail = "missing FNTB section";
                        return false;
                    }
                    uint fntbLength = reader.ReadUInt32();
                    long fimgOffset = fntbOffset + fntbLength;
                    if (fntbLength < 8 || fimgOffset + 8 > fileLength)
                    {
                        detail = "invalid FNTB size";
                        return false;
                    }
                    reader.BaseStream.Position = fimgOffset;
                    if (reader.ReadUInt32() != 0x46494D47)
                    {
                        detail = "missing FIMG section";
                        return false;
                    }
                    uint fimgLength = reader.ReadUInt32();
                    if (fimgLength < 8 || fimgOffset + fimgLength != fileLength)
                    {
                        detail = "invalid FIMG size";
                        return false;
                    }
                    long imageDataLength = fimgLength - 8;

                    reader.BaseStream.Position = fatEntriesOffset;
                    for (uint i = 0; i < count; i++)
                    {
                        uint start = reader.ReadUInt32();
                        uint end = reader.ReadUInt32();
                        if (end < start || end - start != RecordLength || end > imageDataLength)
                        {
                            detail = "member " + i + " is not a bounded 0x34-byte record";
                            return false;
                        }
                    }

                    memberCount = (int)count;
                    detail = memberCount + " valid records";
                    return true;
                }
            }
            catch (Exception ex)
            {
                detail = ex.Message;
                return false;
            }
        }

        private static bool HasThumbVeneer(uint offset, byte byte0, byte byte1, byte byte2, byte byte3)
        {
            try
            {
                byte[] bytes = ARM9.ReadBytes(offset, 8);
                return bytes.Length == 8 &&
                    bytes[0] == byte0 && bytes[1] == byte1 &&
                    bytes[2] == byte2 && bytes[3] == byte3 &&
                    (BitConverter.ToUInt32(bytes, 4) & 1) == 1;
            }
            catch
            {
                return false;
            }
        }

        private static bool TryReadByte(uint offset, out byte value)
        {
            value = 0;
            try
            {
                value = ARM9.ReadByte(offset);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool TryReadUInt32(uint offset, out uint value)
        {
            value = 0;
            try
            {
                byte[] bytes = ARM9.ReadBytes(offset, 4);
                if (bytes.Length != 4) return false;
                value = BitConverter.ToUInt32(bytes, 0);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
