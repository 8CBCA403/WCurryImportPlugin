using CurryImportPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurryImportPlugin.BlockImportUtil
{
    public record class Block(uint Location, string Path)
    {
        public static implicit operator Block((uint, string) v) => new(v.Item1, v.Item2);
    }
    public class SWSHCurryConstants
    {
        public class SH
        {
            public static readonly Block Curry = (0x6EB72940, "CurrySH.bin");
            public static readonly IReadOnlyList<Block> CurryBlocks = new List<Block> { Curry };
        }

        public class SW
        {
            public static readonly Block Curry = (0x6EB72940, "CurrySW.bin");
            public static readonly IReadOnlyList<Block> CurryBlocks = new List<Block> { Curry };
        }
    }
}
