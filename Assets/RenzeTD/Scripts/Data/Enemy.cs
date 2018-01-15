using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using RenzeTD.Scripts.Misc;
using UnityEngine;

namespace RenzeTD.Scripts.Data {
    [DataContract]
    public class EnemyData {
        [DataMember] public ColourType Type;
        [DataMember] public Tuple<float, float, float, float>[] _Colours;

        public Color[] Colours {
            get { return rgbaTocolour(_Colours); }
            set { _Colours = colourTorgba(value); }
        }

        [DataMember]
        public int Health;
        [DataMember]
        public float Speed;

        [Flags]
        public enum ColourType {
            Red, Blue, Green, Yellow, White, Black
        }

        Color[] rgbaTocolour(Tuple<float, float, float, float>[] rgba) {
            List<Color> c = new List<Color>();
            foreach (var f in rgba) {
                c.Add(new Color(f.Item1, f.Item2, f.Item3, f.Item4));
            }
            return c.ToArray();
        }
        Tuple<float, float, float, float>[] colourTorgba(Color[] c) {
            List<Tuple<float, float, float, float>> rgba = new List<Tuple<float, float, float, float>>();
            foreach (var f in c) {
                rgba.Add(new Tuple<float, float, float, float>(f.r, f.g, f.b, f.a));
            }
            return rgba.ToArray();
        }
        
        public EnemyData(ColourType t, Color[] c, int h, float s) {
            Type = t;
            Colours = c;
            Health = h;
            Speed = s;
        }
    }
    
    [DataContract]
    public class Enemy : MonoBehaviour {
        [DataMember] public readonly EnemyData[] Enemies = JsonConvert.DeserializeObject<Enemy>(File.ReadAllText($"{Settings.Instance.EnemyLocation}Enemy.json")).Enemies;


    }
}