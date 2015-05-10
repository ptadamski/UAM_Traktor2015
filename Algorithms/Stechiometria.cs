using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraktorProj.Algorithms
{
    public class Stechiometia<_Input, _Output>
    {
        public Stechiometia(List<_Input> input, List<float> inputAmount, List<_Output> output, List<float> outputAmount)
        {
            if (input.Count == inputAmount.Count && output.Count == outputAmount.Count)
            {
                for (int i = 0; i < input.Count; i++)
                    if (inputAmount[i] != 0.0f)
                        this.input[input[i]] += inputAmount[i];

                for (int i = 0; i < output.Count; i++)
                    if (outputAmount[i] != 0.0f)
                        this.output[output[i]] += outputAmount[i];
            }
        }

        private Dictionary<_Input, float> input;

        private Dictionary<_Output, float> output;     

        public readonly Dictionary<_Input, float> Input
        {
            get { return input; }
        }

        public readonly Dictionary<_Output, float> Output
        {
            get { return output; }
        }

        public void work(Dictionary<_Input, float> input, out Dictionary<_Output, float> output)
        {
            output = new Dictionary<_Output, float>(this.output.Count);

            float efficiency = float.PositiveInfinity;

            foreach (var item in this.input)
            {
                float amount = 0.0f;
                input.TryGetValue(item.Key, out amount);
                efficiency = Math.Min(amount / item.Value, efficiency);
            }

            foreach (var item in input)
                input[item.Key] -= efficiency * item.Value;

            foreach (var item in this.output)
                output[item.Key] = efficiency * item.Value;
        }
    }
}
