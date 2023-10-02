




namespace RealizationOfApp
{
    public static class Shablones
    {
        public static SpecialCircleTextbox GetShablone1()
        {
            SpecialCircleTextbox special = new();
            special.SetRadius(5);
            special.SetFillColorText(new Color(38, 112, 197));
            special.SetFillColorCircle(new Color(21, 101, 192));
            special.SetCharacterSize(15);
            return special;
        }
        public static Textbox GetShabloneTextbox()
        {
            Textbox textbox = new();
            textbox.SetFillColorText(Color.Black);
            textbox.SetFillColorRect(Color.White);
            textbox.SetOutlineColorRect(Color.Black);
            textbox.SetOutlineThicknessRect(2);
            textbox.SetSizeRect(125, 50);
            textbox.SetSizeCharacterText(12);
            textbox.SetString("NO TEXT");
            return textbox;
        }
    }
}
