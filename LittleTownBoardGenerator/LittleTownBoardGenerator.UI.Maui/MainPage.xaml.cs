using LittleTownBoardGenerator.Core;
using Microsoft.Extensions.Options;

namespace LittleTownBoardGenerator.UI.Maui
{
    public partial class MainPage : ContentPage
    {
        public MainPage(){
            InitializeComponent();
            BuildBoard().GetAwaiter().GetResult();
        }

        private async Task BuildBoard()
        {
            try
            {
                DoValidateOptions();

                generateButton.IsEnabled = false;
                resetButton.IsEnabled = false;
                infoLabel.Text = "Generando el tablero...";

                var options = GetOptions();
                var configuration = BoardGenerationConfigurationBuilder.Build(options);
                Board board = null;
                var retries = 0;
                while (board == null && retries < 1000)
                {
                    board = await BoardGenerator.Generate(configuration).ConfigureAwait(false);
                    retries++;
                }
                if (board == null)
                {
                    infoLabel.Text = "No se consiguió generar el tablero.";
                    return;
                }

                Grid grid = (Grid)FindByName("BoardGrid");
                for (var i = 0; i < board.Width; i++)
                {
                    for (var j = 0; j < board.Height; j++)
                    {
                        BoxView box = new BoxView();

                        Color backgroundColor = MapBackgroundColor(board.Squares[i, j]);
                        box.Color = backgroundColor;
                        box.Margin = new Thickness(1, 1, 1, 1);

                        Grid.SetRow(box, j);
                        Grid.SetColumn(box, i);
                        grid.Children.Add(box);
                    }
                }
                infoLabel.Text = "Tablero generado con éxito.";
            }catch(BoardOptionsException ex)
            {
                infoLabel.Text = ex.Message;
            }
            catch(Exception)
            {                
                infoLabel.Text = "Ocurrió un error inesperado.";
            }
            finally
            {
                generateButton.IsEnabled = true;
                resetButton.IsEnabled = true;
            }
        }

        private void DoValidateOptions()
        {
            try
            {
                if (int.Parse(minMountains.Text) > int.Parse(maxMountains.Text))
                    throw new BoardOptionsException("El máximo de montañas no puede ser menor que su mínimo.");

                if (int.Parse(minLakes.Text) > int.Parse(maxLakes.Text))
                    throw new BoardOptionsException("El máximo de lagos no puede ser menor que su mínimo.");

                if (int.Parse(minWoods.Text) > int.Parse(maxWoods.Text))
                    throw new BoardOptionsException("El máximo de bosques no puede ser menor que su mínimo.");

                if (int.Parse(minResources.Text) > int.Parse(maxResources.Text))
                    throw new BoardOptionsException("El máximo de recursos no puede ser menor que su mínimo.");

                if (int.Parse(minMountains.Text) + int.Parse(minLakes.Text) + int.Parse(minWoods.Text) > 54)
                    throw new BoardOptionsException("Los recursos específicados no caben en el tablero.");
            }catch(FormatException)
            {
                throw new BoardOptionsException("Todas las casillas de opciones deben rellenarse.");
            }
        }


        private BoardGenerationConfigurationOptions GetOptions()
        {
            return new BoardGenerationConfigurationOptions(
                9,
                9,
                6,
                6,
                int.Parse(minMountains.Text),
                int.Parse(maxMountains.Text),
                int.Parse(minLakes.Text),
                int.Parse(maxLakes.Text),
                int.Parse(minWoods.Text),
                int.Parse(maxWoods.Text),
                int.Parse(minResources.Text),
                int.Parse(maxResources.Text),
                true,
                false);
        }

        private async void GenerateButton_Clicked(object sender, EventArgs args)
        {
            await BuildBoard().ConfigureAwait(false);
        }

        private async void ResetButton_Clicked(object sender, EventArgs args)
        {
            generateButton.IsEnabled = false;
            resetButton.IsEnabled = false;

            minMountains.Text = "4";
            maxMountains.Text = "4";
            minLakes.Text = "5";
            maxLakes.Text = "5";
            minWoods.Text = "6";
            maxWoods.Text = "6";
            minResources.Text = "0";
            maxResources.Text = "3";

            generateButton.IsEnabled = true;
            resetButton.IsEnabled = true;

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