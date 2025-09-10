using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace _Project.Scripts.SpawnSystems.Editor
{
    public class SpawnTest
    {
        [UnityTest]
        public IEnumerator GIVEN_1_WAVE__START_WAVESYSTEM__SHOULD_FINISH_WAVE()
        {
            // Given
            var settings = new WaveSettings
            {
                Waves = new List<Wave>
                {
                    new Wave
                    {
                        Amount = 2,
                        Delay = 0.2f,
                        Type = 0
                    }
                }
            };
            var fabric = new DummyFabric();
            var wave = new WaveController(settings, fabric, CancellationToken.None);
            
            
            // Act
            var times = 0;
            while (wave.IsFinishAllWaves == false)
            {
                wave.Update(0.1f);
                yield return null;
                times++;
                if (times > 10000)
                    throw new Exception("Entered while true");
            }
            
            // Should
            Assert.IsTrue(wave.IsFinishAllWaves);
        }
        
        [UnityTest]
        public IEnumerator GIVEN_1_WAVE__START_WAVESYSTEM__SHOULD_CALL_LISTENER()
        {
            // Given
            var settings = new WaveSettings
            {
                Waves = new List<Wave>
                {
                    new Wave
                    {
                        Amount = 1,
                        Delay = 0.2f,
                        Type = 0
                    }
                }
            };
            var fabric = new DummyFabric();
            var listener = new DummyUnitListener();
            var wave = new WaveController(settings, fabric, CancellationToken.None);
            wave.RegisterListener(listener);
            
            
            // Act
            var times = 0;
            while (wave.IsFinishAllWaves == false)
            {
                wave.Update(0.1f);
                yield return null;
                times++;
                if (times > 10000)
                    throw new Exception("Entered while true");
            }
            
            // Should
            Assert.IsNotNull(listener.Unit);
        }
        
        [UnityTest]
        public IEnumerator GIVEN_2_WAVEs__START_WAVESYSTEM__SHOULD_FINISH_1ST_AFTER_ALL_DEAD()
        {
            // Given
            var settings = new WaveSettings
            {
                Waves = new List<Wave>
                {
                    new Wave
                    {
                        Amount = 2,
                        Delay = 0.2f,
                        Type = 0
                    },
                    new Wave
                    {
                        Amount = 2,
                        Delay = 0.2f,
                        Type = 1
                    }
                }
            };
            var fabric = new DummyFabric(true);
            var wave = new WaveController(settings, fabric, CancellationToken.None);
            
            
            // Act
            wave.Update(0.1f);
            while (wave.IsSpawning) // wait until it spawns
            {
                yield return null;
            }
            
            Assert.AreEqual(0, wave.CurrentWave); // Not finish because didn't kill

            foreach (var unit in fabric.Units) // kill all units
            {
                unit.IsAlive = false;
            }

            yield return null;
            wave.Update(0.1f);
            yield return null;

            
            // Should
            Assert.AreEqual(1, wave.CurrentWave);
        }

    }

}
