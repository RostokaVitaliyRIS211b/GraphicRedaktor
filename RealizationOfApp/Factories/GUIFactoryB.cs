using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealizationOfApp.Factories
{
    /// <summary>
    /// Фабрика для 3D графики
    /// </summary>
    public class GUIFactoryB:AbstractGUIFactory
    {
        /// <summary>
        /// Фабрика для 3D графики
        /// </summary>
        public GUIFactoryB()
        {

        }
        public override IList<EventDrawableGUI> CreateGUI()
        {
            List<EventDrawableGUI> eventDrawableGUIs = new();
            Textbox shablone = new(Shablones.GetShabloneTextbox());
            
            return eventDrawableGUIs;
        }
        public override bool GetState() => true;
        public override View GetView() => new();

    }
}
