using LumosLIB.Kernel;
using org.dmxc.lumos.Kernel.Input.v2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T = LumosLIB.Tools.I18n.DummyT;

namespace org.dmxc.lumos.Kernel.Input.v2.Worker
{
    public class ResendValueWorker : AbstractNode
    {
        private static readonly string NAME = T._("ResendValue");
        private static readonly string RESENDVALUETYPE = "__RESENDVALUE";

        IGraphNodeInputPort input;
        IGraphNodeOutputPort output;

        public ResendValueWorker(GraphNodeID nodeId) : base(nodeId, RESENDVALUETYPE, KnownCategories.LOGIC)
        {
            this.Name = NAME;

        }

        public override void AddDefaultPorts()
        {
            input = Inputs.SingleOrDefault(c => c.Name == "InputTest") ?? AddInputPort(name: "InputTest");
            input.ShortName = T._("InputTest");
            input.InputValueChanged += ProcessData;
            output = Outputs.SingleOrDefault(c => c.Name == "OutputTest") ?? AddOutputPort(name: "OutputTest");
            output.ShortName = T._("OutputTest");
        }

        private void ProcessData(GraphNodePortID port, object value)
        {
            output.Value = value;
        }
       
    }
}
