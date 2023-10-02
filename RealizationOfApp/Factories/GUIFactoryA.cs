

namespace RealizationOfApp.Factories
{
    public class GUIFactoryA:AbstractGUIFactory
    {
        public override IList<EventDrawableGUI> CreateGUI()
        {
            List<EventDrawableGUI> eventDrawableGUIs = new();
            Textbox shablone = new(Shablones.GetShabloneTextbox());
            ConveirButtonStandart conveirButtonStandart = new();
            ConveirButtonCreatePoint conveirButtonCreatePoint = new();
            ConveirButtonCreateLine conveirButtonCreateLine = new();
            shablone.SetString("Создать точку");
            shablone.SetPos(95, 100);
            EvButton evButtonCreatePoint = new(new(shablone), "CreatePoint");
            conveirButtonStandart.ProcessObj(evButtonCreatePoint);
            conveirButtonCreatePoint.ProcessObj(evButtonCreatePoint);
            eventDrawableGUIs.Add(evButtonCreatePoint);
            shablone.SetString("Создать прямую");
            shablone.SetPos(95, 175);
            EvButton evButtonCreateLine= new(new(shablone), "CreateLine");
            conveirButtonStandart.ProcessObj(evButtonCreateLine);
            conveirButtonCreateLine.ProcessObj(evButtonCreateLine);
            eventDrawableGUIs.Add(evButtonCreateLine);
            return eventDrawableGUIs;
        }
        public override bool GetState() => true;
        public override View GetView() => new();
    }
}
