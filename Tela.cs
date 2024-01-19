using System.Drawing;
using System.Windows.Forms;

namespace JogoWinforms
{
    public abstract class Tela
    {
        public PictureBox PictureBox { get; set; }
        public Graphics Graphics { get; set; }
        public Form MainForm { get; set; }

        public virtual void OnMouseMove(MouseEventArgs e) { }
        public virtual void OnMouseDown(MouseEventArgs e) { }
        public virtual void OnMouseUp(MouseEventArgs e) { }
        public virtual void OnKeyDown(KeyEventArgs e) { }
        public virtual void Carregar() { }
        public virtual void OnTick() { }
    }
}