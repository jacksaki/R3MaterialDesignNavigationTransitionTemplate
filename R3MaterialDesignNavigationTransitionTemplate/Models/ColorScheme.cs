using System.Text.Json.Serialization;

namespace R3MaterialDesignNavigationTransitionTemplate.Models
{
    public enum ColorScheme
    {
        [JsonPropertyName("primary")]
        Primary,
        [JsonPropertyName("secondary")]
        Secondary,
        [JsonPropertyName("primary_foreground")]
        PrimaryForeground,
        [JsonPropertyName("secondary_foreground")]
        SecondaryForeground,
    }
}
