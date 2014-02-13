using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;

namespace VisitorPattern.Automation
{
    [CLSCompliant(false)]
    public static class FindAllTheFiles
    {
        public static IEnumerable<ProjectItem> GetAllTheCsFiles(ProjectItem i)
        {
            if (i.FileCodeModel != null && i.FileCodeModel.Language == CodeModelLanguageConstants.vsCMLanguageCSharp)
            {
                //we have a CSharp file
                return new[] { i };
            }

            if (i.ProjectItems != null)
            {
                return GetAllTheCsFiles(i.ProjectItems);
            }

            return Enumerable.Empty<ProjectItem>();
        }

        public static IEnumerable<ProjectItem> GetAllTheCsFiles(ProjectItems i)
        {
            return i.Cast<ProjectItem>().SelectMany(GetAllTheCsFiles);
        }
    }
}
