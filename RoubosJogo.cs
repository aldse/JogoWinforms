public abstract class RoubosJogo
{
     public int QuantidadeJogadas { get; set; }
    public string Identificacao { get; set; }
    public virtual void pularLinha() { }

    public virtual void atravessarTela() { }
    public virtual void MudarTabuleiro() { }
    public virtual void MudarBolinha() { }
    public virtual void Tunel() { }
    public virtual void ExcluirBolinha() { }

    public virtual void JogadaPossivel() { }
}
