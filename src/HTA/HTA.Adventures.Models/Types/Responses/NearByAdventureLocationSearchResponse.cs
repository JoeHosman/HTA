using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.ServiceInterface.ServiceModel;

namespace HTA.Adventures.Models.Types.Responses
{
    [DataContract]
    public class NearByAdventureLocationSearchResponse : IHasResponseStatus
    {
        [DataMember]
        public NearByAdventureLocationSearch Request { get; set; }

        [DataMember]
        public ResponseStatus ResponseStatus { get; set; }

        [DataMember]
        public List<AdventureLocation> NearByAdventureLocations { get; set; }

        public NearByAdventureLocationSearchResponse(NearByAdventureLocationSearch request)
        {
            Request = request;
            NearByAdventureLocations = new List<AdventureLocation>();
        }

        public override bool Equals(object obj)
        {
            var other = obj as NearByAdventureLocationSearchResponse;
            if (null != other)
            {
                if ((null != Request) != (null != other.Request))
                    return false;

                if (null == Request || null == other.Request)
                    return true;

                if (System.String.CompareOrdinal(Request.LatLon, other.Request.LatLon) != 0)
                    return false;

                return (NearByAdventureLocations.Count == other.NearByAdventureLocations.Count);

            }

            return false;
        }

        public bool Equals(NearByAdventureLocationSearchResponse other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Request, Request) && Equals(other.ResponseStatus, ResponseStatus);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Request != null ? Request.GetHashCode() : 0) * 397) ^ (ResponseStatus != null ? ResponseStatus.GetHashCode() : 0);
            }
        }
    }
}