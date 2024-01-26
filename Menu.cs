using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace JogoWinforms;

public class Menu : Tela
{
    public override void Carregar()
    {
        Pontuacao pontuacao = new Pontuacao();

        Image background = Image.FromFile("img/bom1.png");
        Image jogar = Image.FromFile("img/jogar.png");
        Image sair = Image.FromFile("img/sair.png");

        MainForm.WindowState = FormWindowState.Maximized;
        MainForm.FormBorderStyle = FormBorderStyle.None;
        MainForm.Text = "Joguinho";

        Button ngBtn = new Button();
        ngBtn.FlatStyle = FlatStyle.Flat;
        ngBtn.FlatAppearance.BorderSize = 0;
        ngBtn.FlatAppearance.MouseDownBackColor = Color.Transparent;
        ngBtn.FlatAppearance.MouseOverBackColor = Color.Transparent;
        ngBtn.BackgroundImage = jogar; // Remova esta linha
        ngBtn.BackgroundImageLayout = ImageLayout.Stretch;
        ngBtn.Width = 350;
        ngBtn.Height = 350;
        ngBtn.Location = new Point(670, 680);
        ngBtn.BackColor = Color.Transparent;


        // Arredondar os cantos do botão
        GraphicsPath path = new GraphicsPath();
        int radius = 10;
        path.AddArc(0, 0, radius * 2, radius * 2, 180, 90);
        path.AddArc(ngBtn.Width - 2 * radius, 0, radius * 2, radius * 2, 270, 90);
        path.AddArc(ngBtn.Width - 2 * radius, ngBtn.Height - 2 * radius, radius * 2, radius * 2, 0, 90);
        path.AddArc(0, ngBtn.Height - 2 * radius, radius * 2, radius * 2, 90, 90);
        ngBtn.Region = new Region(path);
        ngBtn.MouseEnter += (sender, e) =>
        {
            ngBtn.ForeColor = Color.Black;
        };

        ngBtn.MouseLeave += (sender, e) =>
        {
            ngBtn.ForeColor = Color.White;
        };

        ngBtn.Click += delegate
        {
            Tela tela = new Jogo
            {
                PictureBox = this.PictureBox,
                MainForm = this.MainForm,
                Graphics = this.Graphics
            };
            tela.Carregar();
            Program.AtualizarTela(tela);
            PictureBox.Controls.Clear();
        };

        Button cntBtn = new Button();
        cntBtn.FlatStyle = FlatStyle.Flat;
        cntBtn.FlatAppearance.BorderSize = 0;
        cntBtn.FlatAppearance.MouseDownBackColor = Color.Transparent;
        cntBtn.FlatAppearance.MouseOverBackColor = Color.Transparent;
        cntBtn.BackgroundImage = sair; // Remova esta linha
        cntBtn.BackgroundImageLayout = ImageLayout.Stretch;
        cntBtn.Width = 300;
        cntBtn.Height = 340;
        cntBtn.Location = new Point(1020, 610);
        cntBtn.BackColor = Color.Transparent;

        // Arredondar os cantos do botão
        GraphicsPath path1 = new GraphicsPath();
        int radius1 = 20; // Ajuste o raio para arredondar mais ou menos
        path1.AddArc(0, 0, radius1 * 2, radius1 * 2, 180, 90);
        path1.AddArc(cntBtn.Width - 2 * radius1, 0, radius1 * 2, radius1 * 2, 270, 90);
        path1.AddArc(cntBtn.Width - 2 * radius1, cntBtn.Height - 2 * radius1, radius1 * 2, radius1 * 2, 0, 90);
        path1.AddArc(0, cntBtn.Height - 2 * radius1, radius1 * 2, radius1 * 2, 90, 90);
        cntBtn.Region = new Region(path1);

        Label melhorPontuacaoLabel = new Label();
        melhorPontuacaoLabel.ForeColor = Color.White;
        melhorPontuacaoLabel.Font = new Font("Comic Sans MS", 20);
        melhorPontuacaoLabel.Text = "Melhor Pontuação: " + pontuacao.ObterMaiorPontuacao();
        melhorPontuacaoLabel.AutoSize = true;
        melhorPontuacaoLabel.Location = new Point(1600, 600);
        PictureBox.Controls.Add(melhorPontuacaoLabel);

        cntBtn.MouseEnter += (sender, e) =>
        {
            cntBtn.ForeColor = Color.Black; // Muda a cor do texto quando o mouse entra no botão
        };

        cntBtn.MouseLeave += (sender, e) =>
        {
            cntBtn.ForeColor = Color.White; // Restaura a cor original do texto quando o mouse sai do botão
        };

        cntBtn.Click += delegate
        {
            Application.Exit();
        };

        PictureBox.BackgroundImageLayout = ImageLayout.Stretch;

        PictureBox.BackgroundImage = background;
        PictureBox.Controls.Add(ngBtn);
        PictureBox.Controls.Add(cntBtn);
    }

    public override void OnKeyDown(KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape)
            Application.Exit();
    }
}