using System;
using System.Windows.Forms;
using NAudio.Wave;

public class GameSound : IDisposable
{
    private WaveOutEvent musicaWaveOut;
    private WaveOutEvent efeitoWaveOut;
    private AudioFileReader audioFile;

    public void PlayMusic(string filePath)
    {
        if (musicaWaveOut == null)
        {
            musicaWaveOut = new WaveOutEvent();
            audioFile = new AudioFileReader(filePath);

            musicaWaveOut.Init(audioFile);

            audioFile.Volume = 0.2f;
          
            musicaWaveOut.Play();
        }
    }
    public void PlayEfeito(string filePath)
    {
        if (efeitoWaveOut == null)
        {
            efeitoWaveOut = new WaveOutEvent();
            audioFile = new AudioFileReader(filePath);

            efeitoWaveOut.Init(audioFile);
            audioFile.Volume = 0.8f;
          
            efeitoWaveOut.Play();
        }
        
    }

    public void StopMusic()
    {
        if (musicaWaveOut != null)
        {
            musicaWaveOut?.Stop();
        }
    }
    public void StopEfeito()
    {
        if (efeitoWaveOut != null)
        {
            efeitoWaveOut?.Stop();
            efeitoWaveOut = null;
        }
    }

    public void Dispose()
    {
        if (musicaWaveOut != null || efeitoWaveOut != null)
        {
            musicaWaveOut.Stop();
            musicaWaveOut.Stop();
            efeitoWaveOut.Dispose();
            musicaWaveOut = null;
            efeitoWaveOut = null;
        }

        if (audioFile != null)
        {
            audioFile.Dispose();
            audioFile = null;
        }
    }
}