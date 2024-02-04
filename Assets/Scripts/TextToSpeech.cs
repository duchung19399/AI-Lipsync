using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Amazon.Polly;
using Amazon.Polly.Model;
using Amazon.Runtime;
using UnityEngine;
using UnityEngine.Networking;

public class TextToSpeech : MonoBehaviour {
    private AmazonPollyClient _client;

    private void Start() {
        var credentials = new BasicAWSCredentials("accessID", "secretKey");
        _client = new AmazonPollyClient(credentials, Amazon.RegionEndpoint.EUCentral1);
    }

    public async void Speak(string text) {
        var request = new SynthesizeSpeechRequest {
            OutputFormat = OutputFormat.Mp3,
            Text = text,
            Engine = Engine.Neural,
            VoiceId = VoiceId.Joanna
        };

        var response = await _client.SynthesizeSpeechAsync(request);
        WriteIntoFile(response.AudioStream);

        using (var www = UnityWebRequestMultimedia.GetAudioClip($"{Application.persistentDataPath}/voice.mp3", AudioType.MPEG)) {
            var op = www.SendWebRequest();

            while (!op.isDone) await Task.Yield();

            var audioClip = DownloadHandlerAudioClip.GetContent(www);
            var audioSource = GetComponent<AudioSource>();
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }

    private void WriteIntoFile(Stream stream) {
        using (var fileStream = new FileStream($"{Application.persistentDataPath}/voice.mp3", FileMode.Create)) {
            byte[] buffer = new byte[8 * 1024];
            int bytesRead;
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0) {
                fileStream.Write(buffer, 0, bytesRead);
            }
        }
    }
}
