using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAMI_SIKON.Model {
    public class EventTrackAssigner {

        private List<Event> _events;
        private List<List<Event>> _tracks;

        public EventTrackAssigner(List<Event> events) {
            Events = events;
        }

        public List<Event> Events {
            get {
                return _events;
            }
            set {
                _events = value;

                UpdateTracks();
            }
        }

        public List<List<Event>> Tracks {
            get {
                return _tracks;
            }
            private set {
                _tracks = value;
            }
        }

        private void UpdateTracks() {
            Tracks = new List<List<Event>>();
            foreach(Event evt in Events) {
                AddToTrack(evt);
            }

            SortTracks();
        }

        private void AddToTrack(Event evt) {
            List<List<Event>> possibleTracks = GetPossibleTracks(evt);
            
            if(possibleTracks.Count == 0) {
                List<Event> track = new List<Event>();
                track.Add(evt);
                Tracks.Add(track);
                return;
            }

            List<List<Event>> thematicTracks = GetThematicallySimilarTracks(evt, possibleTracks);
            if(thematicTracks.Count > 0) {
                possibleTracks = thematicTracks;
            }

            List<Event> trackToAddEventTo = GetLeastCrowdedTrack(possibleTracks);

            trackToAddEventTo.Add(evt);
        }

        private List<List<Event>> GetPossibleTracks(Event evt) {
            List<List<Event>> possibleTracks = new List<List<Event>>();
            foreach(List<Event> track in Tracks) {
                bool overlaps = false;

                foreach(Event e in track) {
                    if(Overlaps(e, evt)) {
                        overlaps = true;
                    }
                }

                if(!overlaps) {
                    possibleTracks.Add(track);
                }
            }
            return possibleTracks;
        }

        private List<List<Event>> GetThematicallySimilarTracks(Event evt, List<List<Event>> tracks) {
            List<List<Event>> thematicTracks = new List<List<Event>>();

            double maxThematicSimilarity = GetMaxThematicSimilarity(evt, tracks);

            foreach(List<Event> track in tracks) {
                if(GetThematicSimilarity(evt, track) == maxThematicSimilarity) {
                    thematicTracks.Add(track);
                }
            }

            return thematicTracks;
        }

        private bool Overlaps(Event evt1, Event evt2) {
            if(evt1.StartTime.CompareTo(evt2.StopTime) > 0 || evt2.StartTime.CompareTo(evt1.StopTime) > 0) {
                return false;
            } else {
                return true;
            }
        }

        private double GetMaxThematicSimilarity(Event evt, List<List<Event>> tracks) {
            double maxThematicSimilarity = 0;
            foreach(List<Event> track in tracks) {
                double thematicSimilarity = GetThematicSimilarity(evt, track);
                if(thematicSimilarity > maxThematicSimilarity) {
                    maxThematicSimilarity = thematicSimilarity;
                }
            }

            return maxThematicSimilarity;
        }

        private double GetThematicSimilarity(Event evt, List<Event> track) {
            int thematicallySimilar = 0;

            foreach (Event e in track) {
                if (e.Theme == evt.Theme) {
                    thematicallySimilar++;
                }
            }
            return (thematicallySimilar / track.Count);
        }

        private List<Event> GetLeastCrowdedTrack(List<List<Event>> tracks) {
            List<Event> uncrowdedTrack = tracks[0];
            foreach(List<Event> track in tracks) {
                if(uncrowdedTrack.Count > track.Count) {
                    uncrowdedTrack = track;
                }
            }
            return uncrowdedTrack;
        }

        private void SortTracks() {
            foreach(List<Event> track in Tracks) {
                track.Sort(
                    Comparer<Event>.Create(
                        (x,y) => x.StartTime > y.StartTime ? 1 : x.StartTime < y.StartTime ? -1 : 0
                        )
                    );
            }
        }
    }
}
