using System.Drawing;
using System.Windows.Forms;

namespace JogoWinforms
{
    public class FormularioJogo : Form
    {
        private Image img = null;
        private Bitmap bmp = null;

        public FormularioJogo()
        {
            Width = Screen.PrimaryScreen.Bounds.Width / 2;
            Height = Screen.PrimaryScreen.Bounds.Height / 2;
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.None;

            BackColor = Color.Plum;
            // // Carrega a imagem do arquivo
            // img = Image.FromFile("img/fundomenu.png");

            // // Define a imagem como fundo do formulário
            // BackgroundImage = img;

            // // Ajusta o layout para cobrir a área do formulário
            // BackgroundImageLayout = ImageLayout.Stretch;
        }
    }
}
