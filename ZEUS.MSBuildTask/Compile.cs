using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;

namespace ZEUS.MSBuildTask
{
    class CompileUnitSystem : Task
    {
        [Required]
        public ITaskItem[] InputFiles { get; set; }

        [Required]
        public ITaskItem OutputDirectory { get; set; }

        [Output]
        public ITaskItem[] OutputFiles { get; set; }

        public override bool Execute()
        {
            foreach (ITaskItem inputFile in InputFiles)
            {
                var outputFile = Compile(inputFile.ItemSpec);
                OutputFiles.Add(outputFile);
            }
        }

        private Compile
    }
}
