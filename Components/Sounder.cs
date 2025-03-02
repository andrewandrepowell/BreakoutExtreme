using Microsoft.Xna.Framework.Audio;
using System;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using MonoGame.Extended.Collections;
using System.Linq;
using BreakoutExtreme.Utility;


namespace BreakoutExtreme.Components
{
    public class Sounder : IUpdate
    {
        private const float _volumeA = 1000;
        private const float _volumeB = 1 / _volumeA;
        private readonly static float _volumeC = (float)(20 * Math.Log10(_volumeB));
        public static float ConvertVolumeForSEI(float volume)
        {
            Debug.Assert(volume >= 0 && volume <= 1);
            var y = (float)(-(20 * Math.Log10(Math.Max(volume, _volumeB)) - _volumeC) / _volumeC);
            return y;
        }
        public enum SoundTypes { SFX, Music }
        public enum Sounds { Brick, BrickBreak, Paddle, Wall, Laser, Empower, Whistle, Cannon, Explosion }
        private enum SoundSamples 
        { 
            Brick0, Brick1, Brick2, Brick3, Brick4, 
            BrickBreak0, BrickBreak1, BrickBreak2, BrickBreak3, BrickBreak4,
            Paddle0, Paddle1, Paddle2, Paddle3, Paddle4,
            Laser0, Laser1, Laser2, Laser3, Laser4,
            Empower0, Empower1, Empower2, Empower3, Empower4,
            Wall0, Whistle0,
            Cannon0, Cannon1, Cannon2,
            Explosion0, Explosion1, Explosion2, Explosion3, Explosion4,
        }
        private class SoundNode(SoundSampleNode[] Nodes, SoundConfig Config)
        {
            private int _currentIndex = -1;
            private SoundSampleNode _currentNode = Nodes[0];
            private readonly SoundSampleNode[] _nodes = Nodes;
            private readonly SoundConfig _config = Config;
            private bool _playing = false;
            public void UpdateVolume()
            {
                _currentNode.SoundEffectInstance.Volume = ConvertVolumeForSEI(
                    _currentNode.Config.Volume *
                    Globals.MasterVolume *
                    (_config.SoundType == SoundTypes.SFX ? Globals.SFXVolume : 
                    (_config.SoundType == SoundTypes.Music ? Globals.MasterVolume : 1)));
            }
            public void Play()
            {
                if (_config.Random)
                    _currentIndex = Globals.Random.Next(_nodes.Length);
                else
                    _currentIndex = (_currentIndex + 1) % _nodes.Length;
                if (_config.Repeat)
                    _playing = true;
                _currentNode = _nodes[_currentIndex];
                _currentNode.SoundEffectInstance.Play();
                UpdateVolume();
            }
            public void Stop()
            {
                if (_config.Repeat)
                    _playing = false;
                _currentNode.SoundEffectInstance.Stop();
                Debug.Assert(_nodes.All(x => x.SoundEffectInstance.State != SoundState.Playing));
            }
            public bool IsPlaying
            {
                get
                {
                    if (_config.Repeat)
                        return _playing;
                    else
                        return _currentNode.SoundEffectInstance.State == SoundState.Playing;
                }
            }
            public void Update()
            {
                if (_config.Repeat && _playing && _currentNode.SoundEffectInstance.State != SoundState.Playing)
                    Play();
            }
        }
        private record SoundConfig(SoundTypes SoundType, SoundSamples[] SoundSamples, bool Random = false, bool Repeat = false);
        private record SoundSampleConfig(string Identifier, float Volume);
        private record SoundSampleNode(SoundEffectInstance SoundEffectInstance, SoundSampleConfig Config);
        private readonly static ReadOnlyDictionary<SoundSamples, SoundSampleConfig> _soundSampleConfigs = new(new Dictionary<SoundSamples, SoundSampleConfig>() 
        {
            { SoundSamples.Brick0, new("sounds/brick_0", 0.5f) },
            { SoundSamples.Brick1, new("sounds/brick_1", 0.5f) },
            { SoundSamples.Brick2, new("sounds/brick_2", 0.5f) },
            { SoundSamples.Brick3, new("sounds/brick_3", 0.5f) },
            { SoundSamples.Brick4, new("sounds/brick_4", 0.5f) },
            { SoundSamples.BrickBreak0, new("sounds/brick_break_0", 0.5f) },
            { SoundSamples.BrickBreak1, new("sounds/brick_break_1", 0.5f) },
            { SoundSamples.BrickBreak2, new("sounds/brick_break_2", 0.5f) },
            { SoundSamples.BrickBreak3, new("sounds/brick_break_3", 0.5f) },
            { SoundSamples.BrickBreak4, new("sounds/brick_break_4", 0.5f) },
            { SoundSamples.Paddle0, new("sounds/paddle_0", 0.1f) },
            { SoundSamples.Paddle1, new("sounds/paddle_1", 0.1f) },
            { SoundSamples.Paddle2, new("sounds/paddle_2", 0.1f) },
            { SoundSamples.Paddle3, new("sounds/paddle_3", 0.1f) },
            { SoundSamples.Paddle4, new("sounds/paddle_4", 0.1f) },
            { SoundSamples.Laser0, new("sounds/laser_0", 0.1f) },
            { SoundSamples.Laser1, new("sounds/laser_1", 0.1f) },
            { SoundSamples.Laser2, new("sounds/laser_2", 0.1f) },
            { SoundSamples.Laser3, new("sounds/laser_3", 0.1f) },
            { SoundSamples.Laser4, new("sounds/laser_4", 0.1f) },
            { SoundSamples.Empower0, new("sounds/empower_0", 0.1f) },
            { SoundSamples.Empower1, new("sounds/empower_1", 0.1f) },
            { SoundSamples.Empower2, new("sounds/empower_2", 0.1f) },
            { SoundSamples.Empower3, new("sounds/empower_3", 0.1f) },
            { SoundSamples.Empower4, new("sounds/empower_4", 0.1f) },
            { SoundSamples.Cannon0, new("sounds/cannon_0", 0.1f) },
            { SoundSamples.Cannon1, new("sounds/cannon_1", 0.1f) },
            { SoundSamples.Cannon2, new("sounds/cannon_2", 0.1f) },
            { SoundSamples.Wall0, new("sounds/wall_0", 0.1f) },
            { SoundSamples.Whistle0, new("sounds/whistle_0", 0.1f) },
            { SoundSamples.Explosion0, new("sounds/explosion_0", 0.1f) },
            { SoundSamples.Explosion1, new("sounds/explosion_1", 0.1f) },
            { SoundSamples.Explosion2, new("sounds/explosion_2", 0.1f) },
            { SoundSamples.Explosion3, new("sounds/explosion_3", 0.1f) },
            { SoundSamples.Explosion4, new("sounds/explosion_4", 0.1f) },
        });
        private readonly static ReadOnlyDictionary<Sounds, SoundConfig> _soundConfigs = new(new Dictionary<Sounds, SoundConfig>() 
        {
            { Sounds.Brick, new(SoundTypes.SFX, [SoundSamples.Brick0, SoundSamples.Brick1, SoundSamples.Brick2, SoundSamples.Brick3, SoundSamples.Brick4], true) },
            { Sounds.BrickBreak, new(SoundTypes.SFX, [SoundSamples.BrickBreak0, SoundSamples.BrickBreak1, SoundSamples.BrickBreak2, SoundSamples.BrickBreak3, SoundSamples.BrickBreak4], true) },
            { Sounds.Paddle, new(SoundTypes.SFX, [SoundSamples.Paddle0, SoundSamples.Paddle1, SoundSamples.Paddle2, SoundSamples.Paddle3, SoundSamples.Paddle4], true) },
            { Sounds.Laser, new(SoundTypes.SFX, [SoundSamples.Laser0, SoundSamples.Laser1, SoundSamples.Laser2, SoundSamples.Laser3, SoundSamples.Laser4], true) },
            { Sounds.Empower, new(SoundTypes.SFX, [SoundSamples.Empower0, SoundSamples.Empower1, SoundSamples.Empower2, SoundSamples.Empower3, SoundSamples.Empower4], true) },
            { Sounds.Cannon, new(SoundTypes.SFX, [SoundSamples.Cannon0, SoundSamples.Cannon1, SoundSamples.Cannon2], true) },
            { Sounds.Wall, new(SoundTypes.SFX, [SoundSamples.Wall0], true) },
            { Sounds.Whistle, new(SoundTypes.SFX, [SoundSamples.Whistle0], true) },
            { Sounds.Explosion, new(SoundTypes.SFX, [SoundSamples.Explosion0, SoundSamples.Explosion1, SoundSamples.Explosion2, SoundSamples.Explosion3, SoundSamples.Explosion4], true, true) },
        });
        private Bag<SoundNode> _soundNodeValues = [];
        private readonly Dictionary<Sounds, SoundNode> _soundNodes = [];
        private readonly Dictionary<SoundSamples, SoundSampleNode> _soundSampleNodes = [];
        private void Load(SoundSamples soundSample)
        {
            Debug.Assert(!_soundSampleNodes.ContainsKey(soundSample));
            var config = _soundSampleConfigs[soundSample];
            var soundEffect = Globals.ContentManager.Load<SoundEffect>(config.Identifier);
            var soundEffectInstance = soundEffect.CreateInstance();
            var soundSampleNode = new SoundSampleNode(soundEffectInstance, config);
            _soundSampleNodes.Add(soundSample, soundSampleNode);
        }
        public void Load(Sounds sound)
        {
            Debug.Assert(!_soundNodes.ContainsKey(sound));
            var config = _soundConfigs[sound];
            var soundSampleNodes = new SoundSampleNode[config.SoundSamples.Length];
            for (var i = 0; i < soundSampleNodes.Length; i++)
            {
                var soundSample = config.SoundSamples[i];
                if (!_soundSampleNodes.ContainsKey(soundSample))
                    Load(soundSample);
                var soundSampleNode = _soundSampleNodes[soundSample];
                soundSampleNodes[i] = soundSampleNode;
            }
            var soundNode = new SoundNode(soundSampleNodes, config);
            _soundNodeValues.Add(soundNode);
            _soundNodes.Add(sound, soundNode);
        }
        public static void Load()
        {
            var sounder = new Sounder();
            foreach (ref var sound in Enum.GetValues<Sounds>().AsSpan())
                sounder.Load(sound);
        }
        public void Play(Sounds sound)
        {
            if (!_soundNodes.ContainsKey(sound))
                Load(sound);
            _soundNodes[sound].Play();
        }
        public void Stop(Sounds sound)
        {
            if (!_soundNodes.ContainsKey(sound))
                Load(sound);
            _soundNodes[sound].Stop();
        }
        public bool IsPlaying(Sounds sound)
        {
            if (!_soundNodes.ContainsKey(sound))
                Load(sound);
            return _soundNodes[sound].IsPlaying;
        }
        public void UpdateVolume()
        {
            for (var i = 0; i < _soundNodeValues.Count; i++)
                _soundNodeValues[i].UpdateVolume();
        }
        public void Update()
        {
            for (var i = 0; i < _soundNodeValues.Count; i++)
                _soundNodeValues[i].Update();
        }
        public Sounder()
        {
            SoundEffect.MasterVolume = 1;
        }
    }
}
