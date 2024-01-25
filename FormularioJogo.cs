using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using JogoWinforms.Roubadas;

namespace JogoWinforms
{
    public class FormularioJogo : Form
    {
        public PictureBox PictureBox { get; set; }
        public Graphics Graphics { get; set; }
        private FlowLayoutPanel acima;
        private FlowLayoutPanel debaixo;
        private List<RoubosJogo> carrinho = new List<RoubosJogo>(); // Lista para armazenar as "roubadas" no carrinho
        private int JogadasFeitas = 100;

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

            // Adiciona 10 cartões superiores
            for (int i = 1; i <= 10; i++)
            {
                var roubo = new AniquilacaoTatica();
                CardRouboJogo(roubo);
            }

            debaixo = new FlowLayoutPanel();
            debaixo.Dock = DockStyle.Bottom;
            debaixo.Height = 120;

            Controls.Add(debaixo);

            // Adiciona 4 cartões inferiores
            for (int i = 1; i <= 4; i++)
            {
                CardInferior(i.ToString());
            }

            Botao();

            debaixo.AllowDrop = true;
            debaixo.DragEnter += new DragEventHandler(Debaixo_DragEnter);
            debaixo.DragDrop += new DragEventHandler(Debaixo_DragDrop);
        }

        private void Debaixo_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(RoubosJogo)))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void Debaixo_DragDrop(object sender, DragEventArgs e)
        {
            var pontoDrop = debaixo.PointToClient(new Point(e.X, e.Y));
            var controleDestino = debaixo.GetChildAtPoint(pontoDrop) as UserControl;
            if (controleDestino != null)
            {
                var rouboJogo = e.Data.GetData(typeof(RoubosJogo)) as RoubosJogo;
                if (rouboJogo != null && JogadasFeitas >= rouboJogo.QuantidadeJogadas && carrinho.Count < 4)
                {
                    carrinho.Add(rouboJogo); 
                    JogadasFeitas -= rouboJogo.QuantidadeJogadas; 
                                                                   
                    controleDestino.Controls.Add(new Label() { Text = rouboJogo.GetType().Name });
                }
            }
        }


        private void Card(string texto)
        {
            var card = new UserControl();
            card.Size = new System.Drawing.Size(180, 120); // Ajusta o tamanho
            card.Margin = new Padding(5); // Adiciona uma margem
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

        private void CardRouboJogo(RoubosJogo rouboJogo)
        {
            var card = new UserControl();
            card.Size = new System.Drawing.Size(180, 120);
            card.Margin = new Padding(5);
            card.BorderStyle = BorderStyle.FixedSingle;
            card.Tag = rouboJogo;

            var label = new Label();
            label.Text = $"{rouboJogo.GetType().Name} - Pontos: {rouboJogo.QuantidadeJogadas}";
            card.Controls.Add(label);

            card.Click += Card_Click; // Adiciona o evento de clique

            card.MouseDown += (sender, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    card.DoDragDrop(rouboJogo, DragDropEffects.Move);
                }
            };

            acima.Controls.Add(card);
        }

        private void Card_Click(object sender, EventArgs e)
        {
            var card = sender as UserControl;
            if (card != null)
            {
                RoubosJogo rouboJogo = card.Tag as RoubosJogo;
                if (rouboJogo != null && JogadasFeitas >= rouboJogo.QuantidadeJogadas && carrinho.Count < 4)
                {
                    carrinho.Add(rouboJogo); // Adiciona ao carrinho
                    JogadasFeitas -= rouboJogo.QuantidadeJogadas; // Deduz os pontos
                    CardInferior(rouboJogo.GetType().Name); // Cria um card no painel debaixo para representar a "roubada" no carrinho
                }
            }
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
