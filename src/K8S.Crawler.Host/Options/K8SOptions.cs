using System.ComponentModel.DataAnnotations;

namespace K8S.Crawler.Host.Options;

public class K8SOptions
{
    public const string ConfigSectionName = "K8S";

    [Required]
    public string Api { get; set; }
}