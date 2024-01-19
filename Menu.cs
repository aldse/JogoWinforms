using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace JogoWinforms;

public class Menu : Tela
{
    public override void Carregar()
    {
        Image botao = Image.FromFile("img/bntj.png");

        MainForm.WindowState = FormWindowState.Maximized;
        MainForm.FormBorderStyle = FormBorderStyle.None;
        MainForm.Text = "Joguinho";

        Button ngBtn = new Button();
        ngBtn.FlatStyle = FlatStyle.Flat;
        ngBtn.FlatAppearance.BorderSize = 0;
        ngBtn.FlatAppearance.MouseDownBackColor = Color.Transparent;
        ngBtn.FlatAppearance.MouseOverBackColor = Color.Transparent;
        ngBtn.BackColor = Color.Black; // Define a cor de fundo inicial do botão
        ngBtn.ForeColor = Color.White; // Define a cor de texto inicial do botão
        ngBtn.Text = "Jogar";
        ngBtn.Font = new Font("Comic Sans MS", 30);
        ngBtn.Width = 230;
        ngBtn.Height = 85;
        ngBtn.Location = new Point(1600, 740);

        // Arredondar os cantos do botão
        GraphicsPath path = new GraphicsPath();
        int radius = 10; // Ajuste o raio para arredondar mais ou menos
        path.AddArc(0, 0, radius * 2, radius * 2, 180, 90);
        path.AddArc(ngBtn.Width - 2 * radius, 0, radius * 2, radius * 2, 270, 90);
        path.AddArc(ngBtn.Width - 2 * radius, ngBtn.Height - 2 * radius, radius * 2, radius * 2, 0, 90);
        path.AddArc(0, ngBtn.Height - 2 * radius, radius * 2, radius * 2, 90, 90);
        ngBtn.Region = new Region(path);
        ngBtn.MouseEnter += (sender, e) =>
        {
            ngBtn.ForeColor = Color.Black; // Muda a cor do texto quando o mouse entra no botão
        };

        ngBtn.MouseLeave += (sender, e) =>
        {
            ngBtn.ForeColor = Color.White; // Restaura a cor original do texto quando o mouse sai do botão
        };

        ngBtn.Click += delegate
        {
            MainForm.Hide();
            // Abrir tela de novo jogo
        };

        Button cntBtn = new Button();
        cntBtn.FlatStyle = FlatStyle.Flat;
        cntBtn.FlatAppearance.BorderSize = 0;
        cntBtn.FlatAppearance.MouseDownBackColor = Color.Transparent;
        cntBtn.FlatAppearance.MouseOverBackColor = Color.Transparent;
        cntBtn.BackColor = Color.Black; // Define a cor de fundo inicial do botão
        cntBtn.ForeColor = Color.White; // Define a cor de texto inicial do botão
        cntBtn.Text = "Sair";
        cntBtn.Font = new Font("Comic Sans MS", 30);
        cntBtn.Width = 230;
        cntBtn.Height = 85;
        cntBtn.Location = new Point(1600, 897);

        // Arredondar os cantos do botão
        GraphicsPath path1 = new GraphicsPath();
        int radius1 = 20; // Ajuste o raio para arredondar mais ou menos
        path1.AddArc(0, 0, radius1 * 2, radius1 * 2, 180, 90);
        path1.AddArc(cntBtn.Width - 2 * radius1, 0, radius1 * 2, radius1 * 2, 270, 90);
        path1.AddArc(cntBtn.Width - 2 * radius1, cntBtn.Height - 2 * radius1, radius1 * 2, radius1 * 2, 0, 90);
        path1.AddArc(0, cntBtn.Height - 2 * radius1, radius1 * 2, radius1 * 2, 90, 90);
        cntBtn.Region = new Region(path1);

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
            MainForm.Hide();
            // Abrir save do usuário
        };

        PictureBox.BackColor = Color.Black;

        PictureBox.Controls.Add(ngBtn);
        PictureBox.Controls.Add(cntBtn);
    }

    public override void OnKeyDown(KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape)
            Application.Exit();
    }
}