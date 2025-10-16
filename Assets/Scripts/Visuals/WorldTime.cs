using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using WorldTime;

namespace WorldTime
{
    public class WorldTime : MonoBehaviour
    {
        [SerializeField]
        private float dayLength;

        private TimeSpan currentTime;
        private float minutelength => dayLength / WorldTimeConstants.minuteInDay;

        private void Start()
        {
            StartCoroutine(addMinute());
        }

        private IEnumerator addMinute()
        {
            currentTime += TimeSpan.FromMinutes(1);
            yield return new WaitForSeconds(minutelength);
            StartCoroutine(addMinute());
        }
    }
}
