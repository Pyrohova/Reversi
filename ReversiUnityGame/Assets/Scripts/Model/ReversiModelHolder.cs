using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReversiCore;
using ReversiCore.Interfaces;
using UnityEngine;
using Unity;

namespace Assets.Scripts.Model
{
    public class ReversiModelHolder : MonoBehaviour
    {
        //IReversiModel reversiModel = ContainerHolder.Source.Container.Resolve<IReversiModel>();
        public ReversiModel reversiModel { get; private set; }  = new ReversiModel();
    }
}
