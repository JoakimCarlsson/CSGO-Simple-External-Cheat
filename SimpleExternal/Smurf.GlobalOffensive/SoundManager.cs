using System;
using System.IO;
using SharpDX.Multimedia;
using SharpDX.XAudio2;

namespace Smurf.GlobalOffensive
{
    public class SoundManager : IDisposable
    {
        #region Constructor

        public SoundManager(int amountOfSounds)
        {
            _audio = new XAudio2();
            _masteringVoice = new MasteringVoice(_audio);
            _masteringVoice.SetVolume(0.5f);
            _soundStreams = new SoundStream[amountOfSounds];
            _audioBuffers = new AudioBuffer[amountOfSounds];
            _sourceVoices = new SourceVoice[amountOfSounds];
        }

        #endregion

        #region Variables

        private XAudio2 _audio;
        private readonly MasteringVoice _masteringVoice;
        private readonly SoundStream[] _soundStreams;
        private readonly AudioBuffer[] _audioBuffers;
        private readonly SourceVoice[] _sourceVoices;

        #endregion

        #region Methods

        //Adds the sound we want to play
        public void Add(int index, UnmanagedMemoryStream stream)
        {
            _soundStreams[index] = new SoundStream(stream);
            _audioBuffers[index] = new AudioBuffer
            {
                Stream = _soundStreams[index].ToDataStream(),
                AudioBytes = (int) _soundStreams[index].Length,
                Flags = BufferFlags.EndOfStream
            };
            _sourceVoices[index] = new SourceVoice(_audio, _soundStreams[index].Format);
        }

        //Plays the sound
        public void Play(int index)
        {
            _sourceVoices[index].Stop();
            _sourceVoices[index].FlushSourceBuffers();
            _sourceVoices[index].SubmitSourceBuffer(_audioBuffers[index], _soundStreams[index].DecodedPacketsInfo);
            _sourceVoices[index].Start();
        }

        //Sets the voulme of the sound we'll play.
        public void SetVolume(float volume)
        {
            _masteringVoice.SetVolume(volume);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (_audio != null)
            {
                _audio.StopEngine();
                _audio.Dispose();
                _audio = null;

                foreach (var sourceVoice in _sourceVoices)
                {
                    sourceVoice.Dispose();
                }
                foreach (var soundStream in _soundStreams)
                {
                    soundStream.Dispose();
                }
            }
        }

        #endregion
    }
}