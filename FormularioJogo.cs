using System;
using System.Drawing;
using System.Windows.Forms;

namespace JogoWinforms
{
    public class FormularioJogo : Form
    {
        public PictureBox PictureBox { get; set; }
        public Graphics Graphics { get; set; }
        private FlowLayoutPanel acima;
        private FlowLayoutPanel debaixo;

        public FormularioJogo()
        {
            PictureBox = new PictureBox();
            PictureBox.Width = 800;
            PictureBox.Height = 600;

            Width = Screen.PrimaryScreen.Bounds.Width / 2;
            Height = Screen.PrimaryScreen.Bounds.Height / 2;
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.None;

            acima = new FlowLayoutPanel();
            acima.Dock = DockStyle.Fill;
            acima.AutoScroll = true;

            Controls.Add(acima);

            Card("Card 1");
            Card("Card 3");
            Card("Card 5");
            Card("Card 7");
            Card("Card 11");
            Card("Card 20");
            Card("Card 24");
            Card("Card 28");
            Card("Card 31");
            Card("Card 35");
            Card("Card 39");
            Card("Card 47");

            debaixo = new FlowLayoutPanel();
            debaixo.Dock = DockStyle.Bottom;
            debaixo.Height = 120; 

            Controls.Add(debaixo);

            CardInferior("1");
            CardInferior("2");
            CardInferior("3");
            CardInferior("4");

            Botao();
        }

        private void Card(string texto)
        {
            var card = new UserControl();
            card.Size = new System.Drawing.Size(150, 100);
            card.BorderStyle = BorderStyle.FixedSingle;

            var label = new Label();
            label.Text = texto;
            card.Controls.Add(label);

            acima.Controls.Add(card);
        }

        private void CardInferior(string texto)
        {
            var card = new UserControl();
            card.Size = new System.Drawing.Size(150, 100);
            card.BorderStyle = BorderStyle.FixedSingle;

            var label = new Label();
            label.Text = texto;
            card.Controls.Add(label);

            debaixo.Controls.Add(card);
        }


        public void Botao()
        {
            Button sair = new Button();
            sair.FlatStyle = FlatStyle.Flat;
            sair.FlatAppearance.BorderSize = 0;
            sair.FlatAppearance.MouseDownBackColor = Color.Transparent;
            sair.FlatAppearance.MouseOverBackColor = Color.Transparent;
            sair.BackColor = Color.Black; 
            sair.ForeColor = Color.Red;
            sair.Font = new Font("Arial", 12);
            sair.Text = "Menu";
            sair.Size = new Size(200, 30);
            sair.Width = 230;
            sair.Height = 85;
            sair.Location = new Point(PictureBox.Width - 10, PictureBox.Height - 80);

            sair.Click += delegate
            {
                this.Close();
            };

            Controls.Add(sair);
        }

    }
}
