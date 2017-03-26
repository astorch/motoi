using BMF.NShape;
using Motoi.Platform;
using Motoi.SharpFace.Wizards;
using Motoi.UI;
using Motoi.UI.Shells;
using Motoi.UI.Widgets;

namespace SharpFlow
{
    public class NewModelWizard : AbstractWizard
    {
        protected override void OnInitialize() {
            Title = "Create a new Model";
        }

        protected override void OnCancel() {
            ITitledAreaDialog dialog = UIFactory.NewViewPart<ITitledAreaDialog>();
            IGridComposite gridComposite = UIFactory.NewWidget<IGridComposite>();
            gridComposite.GridColumns = 1;
            gridComposite.GridRows = 1;
            gridComposite.AddWidget(new Canvas { ShowGrid = true });
            dialog.ContentPane = gridComposite;
            dialog.Show();
        }

        protected override void OnFinish() {
            PlatformUI.ActivePerspective.OpenEditor();
        }

        protected override void OnDispose() {
            
        }
    }
}