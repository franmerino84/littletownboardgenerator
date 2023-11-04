using LittleTownBoardGenerator.Core;
using Microsoft.Extensions.Options;

namespace LittleTownBoardGenerator.UI.Maui
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BuildBoard().GetAwaiter().GetResult();
        }

        private async Task BuildBoard()
        {
            try
            {
                generateButton.IsEnabled = false;
                resetButton.IsEnabled = false;

                DoValidateGeneralConfiguration();

                infoLabel.Text = "Generando el tablero...";

                
                var generalConfiguration = GetGeneralConfiguration();

                var cancellationTokenSource = new CancellationTokenSource();
                var timer = new Timer(CancelTask, cancellationTokenSource, TimeSpan.FromSeconds(5), TimeSpan.FromMilliseconds(-1));

                var board = await BoardGenerator.Generate(generalConfiguration, cancellationTokenSource.Token);

                if (board == null)
                {
                    infoLabel.Text = "No se puede generar un tablero con esas opciones.";
                    return;
                }

                Grid grid = (Grid)FindByName("BoardGrid");
                for (var i = 0; i < board.Width; i++)
                {
                    for (var j = 0; j < board.Height; j++)
                    {
                        BoxView box = new();

                        Color backgroundColor = MainPage.MapBackgroundColor(board.Squares[i, j]);
                        box.Color = backgroundColor;
                        box.Margin = new Thickness(1, 1, 1, 1);

                        Grid.SetRow(box, j);
                        Grid.SetColumn(box, i);
                        grid.Children.Add(box);
                    }
                }
                infoLabel.Text = "Tablero generado con éxito.";
            }
            catch (BoardGenerationCancelledException)
            {
                infoLabel.Text = "Tardó mucho en generar el tablero. Reduce opciones.";
            }
            catch (BoardGeneralConfigurationException ex)
            {
                infoLabel.Text = ex.Message;
            }
            catch (Exception)
            {
                infoLabel.Text = "Ocurrió un error inesperado.";
            }
            finally
            {
                generateButton.IsEnabled = true;
                resetButton.IsEnabled = true;
            }
        }

        private void CancelTask(object state)
        {
            var cancellationTokenSource = (CancellationTokenSource)state;
            cancellationTokenSource.Cancel();
        }

        private void DoValidateGeneralConfiguration()
        {
            try
            {
                if (int.Parse(minMountains.Text) > int.Parse(maxMountains.Text))
                    throw new BoardGeneralConfigurationException("El máximo de montañas no puede ser menor que su mínimo.");

                if (int.Parse(minLakes.Text) > int.Parse(maxLakes.Text))
                    throw new BoardGeneralConfigurationException("El máximo de lagos no puede ser menor que su mínimo.");

                if (int.Parse(minWoods.Text) > int.Parse(maxWoods.Text))
                    throw new BoardGeneralConfigurationException("El máximo de bosques no puede ser menor que su mínimo.");

                if (int.Parse(minResources.Text) > int.Parse(maxResources.Text))
                    throw new BoardGeneralConfigurationException("El máximo de recursos no puede ser menor que su mínimo.");

                if (int.Parse(minMountains.Text) + int.Parse(minLakes.Text) + int.Parse(minWoods.Text) > 54)
                    throw new BoardGeneralConfigurationException("Los recursos específicados no caben en el tablero.");
            }
            catch (FormatException)
            {
                throw new BoardGeneralConfigurationException("Todas las casillas de opciones deben rellenarse bien.");
            }
        }


        private BoardGenerationGeneralConfiguration GetGeneralConfiguration()
        {
            return new BoardGenerationGeneralConfiguration(
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

        private void ResetButton_Clicked(object sender, EventArgs args)
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

        private static Color MapBackgroundColor(Square square)
        {
            return square switch
            {
                Square.Nothing => Color.FromRgba(255, 255, 255, 255),
                Square.Lake => Color.FromRgba(0, 0, 255, 255),
                Square.Wood => Color.FromRgba(0, 255, 0, 255),
                Square.Mountain => Color.FromRgba(255, 0, 0, 255),
                _ => Color.FromRgba(0, 0, 0, 255),
            };
        }
    }
}