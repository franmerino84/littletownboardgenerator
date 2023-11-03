using LittleTownBoardGenerator.Core;

namespace LittleTownBoardGenerator.Maui
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BuildBoard();
        }

        private void BuildBoard()
        {
            var board = BoardGenerator.Generate(new BoardGenerationConfiguration());
            Grid grid = (Grid)FindByName("BoardGrid");
            for(var i = 0; i < board.Width; i++)
            {
                for(var j = 0; j < board.Height; j++)
                {
                    BoxView box = new BoxView();

                    Color backgroundColor = MapBackgroundColor(board.Squares[i,j]);
                    box.Color = backgroundColor;
                    box.Margin = new Thickness(1, 1, 1, 1);
                    

                    Grid.SetRow(box, i);
                    Grid.SetColumn(box, j);
                    //grid.Children.Clear();
                    grid.Children.Add(box);
                }
            }
            
        }

        private async void GenerateButton_Clicked(object sender, EventArgs args)
        {
            BuildBoard();

        }

        private Color MapBackgroundColor(Square square)
        {
            switch (square)
            {
                case Square.Nothing:
                    return Color.FromRgba(255, 255, 255, 255);
                case Square.Lake:
                    return Color.FromRgba(0, 0, 255, 255);
                case Square.Wood:
                    return Color.FromRgba(0, 255, 0, 255);
                case Square.Mountain:
                    return Color.FromRgba(255, 0, 0, 255);
                default:
                    return Color.FromRgba(0, 0, 0, 255);
            }
        }
    }
}