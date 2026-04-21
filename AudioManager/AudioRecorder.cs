using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace AudioManager
{
    public sealed class AudioRecorder : IAudioRecorder
    {
        private WasapiCapture? microphoneCapture;
        private WasapiLoopbackCapture? speakerCapture;
        private WaveFileWriter? writer;
        private WaveFormat? outputFormat;
        private DateTime recordingStartTime;
        private bool disposed;
        private readonly object writeLock = new();

        public bool IsRecording { get; private set; }

        public TimeSpan Elapsed =>
            IsRecording ? DateTime.Now - this.recordingStartTime : TimeSpan.Zero;

        public void StartRecording(string outputFilePath)
        {
            if (IsRecording)
            {
                throw new InvalidOperationException("Already recording.");
            }

            this.outputFormat = new WaveFormat(44100, 16, 2);
            this.writer = new WaveFileWriter(outputFilePath, this.outputFormat);

            this.microphoneCapture = new WasapiCapture();
            this.microphoneCapture.DataAvailable += OnMicrophoneDataAvailable;

            this.speakerCapture = new WasapiLoopbackCapture();
            this.speakerCapture.DataAvailable += OnSpeakerDataAvailable;

            this.recordingStartTime = DateTime.Now;
            IsRecording = true;

            this.microphoneCapture.StartRecording();
            this.speakerCapture.StartRecording();
        }

        public void StopRecording()
        {
            if (!IsRecording)
            {
                return;
            }

            IsRecording = false;

            this.microphoneCapture?.StopRecording();
            this.speakerCapture?.StopRecording();

            lock (this.writeLock)
            {
                this.writer?.Dispose();
                this.writer = null;
            }

            CleanupCapture();
        }

        private void OnMicrophoneDataAvailable(object? sender, WaveInEventArgs e)
        {
            WriteResampled(this.microphoneCapture!.WaveFormat, e.Buffer, e.BytesRecorded);
        }

        private void OnSpeakerDataAvailable(object? sender, WaveInEventArgs e)
        {
            WriteResampled(this.speakerCapture!.WaveFormat, e.Buffer, e.BytesRecorded);
        }

        private void WriteResampled(WaveFormat sourceFormat, byte[] buffer, int bytesRecorded)
        {
            if (bytesRecorded == 0)
            {
                return;
            }

            lock (this.writeLock)
            {
                if (this.writer == null || this.outputFormat == null)
                {
                    return;
                }

                using var inputStream = new RawSourceWaveStream(buffer, 0, bytesRecorded, sourceFormat);
                var sampleProvider = inputStream.ToSampleProvider();

                if (sourceFormat.Channels == 1)
                {
                    sampleProvider = new MonoToStereoSampleProvider(sampleProvider);
                }

                var resampler = new WdlResamplingSampleProvider(sampleProvider, this.outputFormat.SampleRate);
                var resampledBuffer = new float[bytesRecorded];
                var samplesRead = resampler.Read(resampledBuffer, 0, resampledBuffer.Length);

                if (samplesRead > 0)
                {
                    this.writer.WriteSamples(resampledBuffer, 0, samplesRead);
                }
            }
        }

        private void CleanupCapture()
        {
            if (this.microphoneCapture != null)
            {
                this.microphoneCapture.DataAvailable -= OnMicrophoneDataAvailable;
                this.microphoneCapture.Dispose();
                this.microphoneCapture = null;
            }

            if (this.speakerCapture != null)
            {
                this.speakerCapture.DataAvailable -= OnSpeakerDataAvailable;
                this.speakerCapture.Dispose();
                this.speakerCapture = null;
            }
        }

        public void Dispose()
        {
            if (!this.disposed)
            {
                StopRecording();
                this.disposed = true;
            }
        }
    }
}
