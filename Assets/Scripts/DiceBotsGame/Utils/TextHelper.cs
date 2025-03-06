using System.Linq;

namespace DiceBotsGame.Utils {
   public static class TextHelper {
      public static string WithInjectedParameters(this string text, params (string key, object value)[] parameters) =>
         parameters.Aggregate(text, (current, parameter) => current.Replace($"{{{parameter.key}}}", parameter.value.ToString()));
   }
}