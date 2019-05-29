namespace AdminCore.DTOs.LinkGenerator
{
    public class HashedLinkDto
    {
        public HashedLinkDto(
            string domain,
            string route,
            string queryHash,
            string protocol = "https")
        {
            Protocol = protocol;
            Domain = domain;
            Route = route;
            QueryHash = queryHash;
        }

        public string Protocol { get; set; }
        public string Domain { get; set; }
        public string Route { get; set; }
        public string QueryHash { get; set; }
        
        public string HashedLink => $"{Protocol}://{Domain}/{Route}/{QueryHash}";
    }
}
