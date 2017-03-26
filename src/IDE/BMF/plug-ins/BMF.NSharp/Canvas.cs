using System;
using Dataweb.NShape;
using Dataweb.NShape.Advanced;
using Dataweb.NShape.Controllers;
using Dataweb.NShape.WinFormsUI;
using Motoi.UI.Widgets;

namespace BMF.NShape
{
    public class Canvas : Display, IWidget {

        // Step 1
        private Project project = new Project();
        private DiagramSetController Controller = new DiagramSetController();
        private CachedRepository Repository = new CachedRepository();
        private XmlStore xmlStore = new XmlStore();

        // Step 2
        private ToolSetController ToolSetController = new ToolSetController();
        protected override void OnLoad(System.EventArgs e) {
            base.OnLoad(e);

            Controller.Project = project;
            DiagramSetController = Controller;
            project.Repository = Repository;
            Repository.Store = xmlStore;

            ToolSetController.DiagramSetController = Controller;

            xmlStore.DirectoryName = @"C:\Users\Public\Documents\NShape\Demo Projects";
            //@"C:\Documents and Settings\All Users\Common Files\NShape\Demo Projects";
            xmlStore.FileExtension = "nspj";
            project.Name = "Circles";
            // @"C:\Documents and Settings\All Users\Common Files\NShape\bin\Debug"
            project.LibrarySearchPaths.Add(@"C:\Users\Public\Documents\NShape\bin\Debug");
            project.AutoLoadLibraries = true;
            try
            {
                project.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            LoadDiagram("Diagram 1");
        }
    }
}