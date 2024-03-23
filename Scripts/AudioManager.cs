using Godot;
using System;
using System.Collections.Generic;
using System.Reflection;

public partial class AudioManager : Node {

	public static AudioManager Instance { get; private set; }

	[Export] private AudioStream[] audioStreams;
	[Export] private ProjectilePool globalAudioPlayers;
	[Export] private ProjectilePool localAudioPlayers;

	private List<AudioStreamPlayer> activeGlobalAudio;
	private List<AudioStreamPlayer2D> activeLocalAudio;

	public override void _Ready() {
		if (Instance != null) QueueFree();
		else {
			Instance = this;

			activeGlobalAudio = new List<AudioStreamPlayer>();
			activeLocalAudio = new List<AudioStreamPlayer2D>();

		}
	}

	public override void _ExitTree() {
		if (Instance == this) {
			Instance = null;
		}
	}

	public override void _Process(double delta) {
		for (int i = activeGlobalAudio.Count - 1; i >= 0; i--) {
			if (activeGlobalAudio[i].GetPlaybackPosition() >= activeGlobalAudio[i].Stream.GetLength()) {
				globalAudioPlayers.Despawn(activeGlobalAudio[i]);
				activeGlobalAudio.RemoveAt(i);
			}
		}

		for (int i = activeLocalAudio.Count - 1; i >= 0; i--) {
			if (activeLocalAudio[i].GetPlaybackPosition() >= activeLocalAudio[i].Stream.GetLength()) {
				localAudioPlayers.Despawn(activeLocalAudio[i]);
				activeLocalAudio.RemoveAt(i);
			}
		}
	}

	public AudioStreamPlayer PlayGlobal(AudioStream audio) {
		if (globalAudioPlayers.Spawn() is AudioStreamPlayer gPlayer && gPlayer != null) {
			AddChild(gPlayer);

			gPlayer.Stream = audio;
			gPlayer.Play(0f);

			activeGlobalAudio.Add(gPlayer);

			return gPlayer;
		}
		return null;
	}

	public AudioStreamPlayer2D PlayLocal(AudioStream audio, Vector2 positon) {
		if (localAudioPlayers.Spawn() is AudioStreamPlayer2D lPlayer && lPlayer != null) {
			AddChild(lPlayer);
			lPlayer.GlobalPosition = positon;

			lPlayer.Stream = audio;
			lPlayer.Play(0f);

			activeLocalAudio.Add(lPlayer);

			return lPlayer;
		}
		return null;
	}

	public AudioStreamPlayer PlayGlobal(int index) {
		return PlayGlobal(audioStreams[index]);
	}

	public AudioStreamPlayer2D PlayLocal(int index, Vector2 positon) {
		return PlayLocal(audioStreams[index], positon);
	}

}
