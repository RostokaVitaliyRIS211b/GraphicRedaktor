

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
            ConveirButtonRotation conveirButtonRotation = new();
            ConveirButtonMirroring conveirButtonMirroring = new();
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
            shablone.SetString("Вращение");
            shablone.SetPos(95, 250);
            EvButton evButtonRotate = new(new(shablone), "Rotate");
            conveirButtonStandart.ProcessObj(evButtonRotate);
            conveirButtonRotation.ProcessObj(evButtonRotate);
            eventDrawableGUIs.Add(evButtonRotate);
            shablone.SetString("Зеркалирование");
            shablone.SetPos(95, 325);
            EvButton evButtonMirroring = new(new(shablone), "Mirroring");
            conveirButtonStandart.ProcessObj(evButtonMirroring);
            conveirButtonMirroring.ProcessObj(evButtonMirroring);
            eventDrawableGUIs.Add(evButtonMirroring);
            return eventDrawableGUIs;
        }
        public override bool GetState() => true;
        public override View GetView() => new();
    }
}
