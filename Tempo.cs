using System;
using System.Threading;

namespace JogoWinforms
{
    public class Tempo
    {
        private Timer temporizador;
        private int tempoRestante;

        public Tempo()
        {
            temporizador = new Timer(Temporizador, null, 1000, 1000);
            ResetarTempo(); 
        }
        /// <summary>
        /// Evento para notificar que o tempo esgotou
        /// </summary>
        public event EventHandler TempoEsgotado; 

        private void Temporizador(object state)
        {
            tempoRestante--;

            if (tempoRestante <= 0)
            {
                OnTempoEsgotado();
                ResetarTempo();
            }
        }

        private void OnTempoEsgotado()
        {
            TempoEsgotado?.Invoke(this, EventArgs.Empty);
        }

        private void ResetarTempo()
        {
            tempoRestante = 60; 
        }
    }
}
