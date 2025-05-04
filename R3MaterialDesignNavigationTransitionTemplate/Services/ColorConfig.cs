using R3MaterialDesignNavigationTransitionTemplate.Models;
using System.Text.Json.Serialization;
using System.Windows.Media;

namespace R3MaterialDesignNavigationTransitionTemplate.Services
{
    public class ColorConfig
    {
        [JsonPropertyName("scheme")]
        public ColorScheme Scheme { get; set; }
        [JsonPropertyName("color")]
        public Color? Color { get; set; }
    }
}
