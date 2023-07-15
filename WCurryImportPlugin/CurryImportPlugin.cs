using CurryImportPlugin.BlockImportUtil;
using PKHeX.Core;
using System.Linq;
using System.Threading.Tasks;
namespace CurryImportPlugin
{
    public class CurryImportPlugin : IPlugin
    {
            public string Name => nameof(CurryImportPlugin);
            public int Priority => 1;
            public ISaveFileProvider SaveFileEditor { get; private set; } = null!;
            public IPKMView PKMEditor { get; private set; } = null!;
            private ToolStripMenuItem ImportCurryButton = null!;
            private bool IsCompatibleSave
            {
                get { return SaveFileEditor.SAV is SAV8SWSH; }
            }


        public void Initialize(params object[] args)
            {
                Console.WriteLine($"Loading {Name}...");
                SaveFileEditor = (ISaveFileProvider)Array.Find(args, z => z is ISaveFileProvider)!;
                PKMEditor = (IPKMView)Array.Find(args, z => z is IPKMView)!;
                ToolStrip menu = (ToolStrip)Array.Find(args, z => z is ToolStrip)!;
                ToolStripDropDownItem tools = (ToolStripDropDownItem)menu.Items.Find("Menu_Tools", false)[0]!;
                ImportCurryButton = new ToolStripMenuItem("咖喱导入插件");
                ImportCurryButton.Available = IsCompatibleSave;
                ImportCurryButton.Click += (s, e) => ImportClothes();
                tools.DropDownItems.Add(ImportCurryButton);

            }

        private void ImportClothes()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedPath = folderBrowserDialog.SelectedPath;
                IReadOnlyList<Block>? readOnlyList = null;
                if (SaveFileEditor.SAV is SAV8SWSH)
                {
                    if (SaveFileEditor.SAV.Version == GameVersion.SW)
                    {
                        readOnlyList = SWSHCurryConstants.SW.CurryBlocks;
                    }
                    else if (SaveFileEditor.SAV.Version == GameVersion.SH)
                    {
                        readOnlyList = SWSHCurryConstants.SH.CurryBlocks;
                    }

                }
                ImportCurry(selectedPath, (dynamic)SaveFileEditor.SAV, readOnlyList);
            }
        }
        private static void ImportCurry<S>(string raidPath, S sav, IReadOnlyList<Block> blocks) where S : SaveFile, ISCBlockArray, ISaveFileRevision
        {
            string CurryFilePath(string file) => $@"{raidPath}\{file}";
            if (blocks.All(b => File.Exists(CurryFilePath(b.Path))))
            {
                foreach ((uint blockLocation, string file) in blocks)
                    sav.Accessor.GetBlock(blockLocation).ChangeData(File.ReadAllBytes(CurryFilePath(file)));
                sav.State.Edited = true;
                MessageBox.Show("咖喱导入成功", "咖喱已导入");
            }
            else
            {
                MessageBox.Show($@"确保所有必需的文件都在 {raidPath}", "咖喱未导入", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string? CurryFilePath(object pathPath)
        {
            throw new NotImplementedException();
        }

        public void NotifySaveLoaded() => ImportCurryButton.Available = IsCompatibleSave;
        public bool TryLoadFile(string filePath) => false;
    }
}