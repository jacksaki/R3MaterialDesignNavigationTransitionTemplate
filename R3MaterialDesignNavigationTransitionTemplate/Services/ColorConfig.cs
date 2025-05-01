using System.Text.Json.Serialization;
using System.Windows.Media;
using R3MaterialDesignNavigationTransitionTemplate.Models;

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
