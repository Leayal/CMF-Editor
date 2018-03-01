using System.Collections;
using static CMF_Editor.Extraction;

namespace CMF_Editor.Classes
{
    public class ExtractionParams
    {
        public CMFFile Archive { get; set; }
        public ExtractionOptions ExtractionOptions { get; set; }
        public string Destination { get; set; }
        public IList SelectedFiles { get; set; }
    }
    public struct ExtractionOptions
    {
        public bool OptionContinueOnError { get; set; }
        public FilePathType OptionFilePathType { get; set; }
        public bool OptionDisplayFileAfterExtract { get; set; }
        public bool OptionToSubfolder { get; set; }
        public OverwriteMode OptionOverwriteMode { get; set; }
        public UpdateMode OptionUpdateMode { get; set; }
}
}
