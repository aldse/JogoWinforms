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

            for (int i = 1; i <= 10; i++)
            {
                var roubo = new AniquilacaoTatica();
                CardRouboJogo(roubo);
            }

            debaixo = new FlowLayoutPanel();
            debaixo.Dock = DockStyle.Bottom;
            debaixo.Height = 120;

            Controls.Add(debaixo);

            for (int i = 1; i <= 4; i++)
            {
                CardInferior(i.ToString());
            }

            Load += (_, _) => Botao();

            debaixo.AllowDrop = true;
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

        private void CardRouboJogo(RoubosJogo rouboJogo) //so 1 ate agr
        {
            var card = new UserControl();
            card.Size = new System.Drawing.Size(180, 120);
            card.Margin = new Padding(5);
            card.BorderStyle = BorderStyle.FixedSingle;
            card.Tag = rouboJogo;

            var label = new Label();
            label.Text = $"{rouboJogo.GetType().Name} - Pontos: {rouboJogo.QuantidadeJogadas}";
            card.Controls.Add(label);

            card.Click += Card_Click;

            card.MouseDown += (sender, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    card.DoDragDrop(rouboJogo, DragDropEffects.Move);
                }
            };

            acima.Controls.Add(card);
        }

        private void Card_Click(object card, EventArgs e)
        {
            var carta = card as UserControl;
            if (card != null)
            {
                RoubosJogo rouboJogo = carta.Tag as RoubosJogo;
                if (rouboJogo != null)
                {

                    if (JogadasFeitas >= rouboJogo.QuantidadeJogadas && carrinho.Count < 4)
                    {
                        JogadasFeitas -= rouboJogo.QuantidadeJogadas;

                        MoverCardParaBaixo(rouboJogo);
                    }
                    else if (carrinho.Count >= 4)
                    {
                        MessageBox.Show("Você já tem o número máximo de roubadas para por em jogo!");
                    }
                    else
                    {
                        MessageBox.Show("Você não tem jogadas suficientes!");
                    }
                }
            }
        }

        /// <summary>
        /// Por o card de cima para baixo
        /// </summary>
        /// <param name="rouboJogo"></param>
        private void MoverCardParaBaixo(RoubosJogo rouboJogo)
        {
            if (debaixo.Controls.Count < 4) // Verifica se há espaço no painel de baixo
            {
                UserControl cardInferior = new UserControl();
                cardInferior.Size = new System.Drawing.Size(150, 100);
                cardInferior.BorderStyle = BorderStyle.FixedSingle;

                var label = new Label();
                label.Text = rouboJogo.GetType().Name;
                cardInferior.Controls.Add(label);

                debaixo.Controls.Add(cardInferior); // Adiciona o novo card ao painel de baixo
            }
            else
            {
                MessageBox.Show("Você já tem o número máximo de roubadas no carrinho!");
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
            sair.Location = new Point(PictureBox.Width - 80, PictureBox.Height - 10);

            sair.Click += delegate
            {
                this.Close();
            };

            PictureBox.Controls.Add(sair);
        }

    }
}
