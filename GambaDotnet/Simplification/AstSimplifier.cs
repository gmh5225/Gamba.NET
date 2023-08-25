using Gamba.Ast;
using Gamba.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamba.Simplification
{
    public class AstSimplifier
    {
        private int cost;

        private AstNode root;

        private IReadOnlyDictionary<AstNode, AstClassification> classificationMapping;

        public AstSimplifier(AstNode root)
        {
            this.root = root;
            cost = root.ToString().Length;
            classificationMapping = AstClassifier.Classify(root);
        }

        public void Simplify()
        {
            SimplifyLinearSubtrees();
        }

        private void SimplifyLinearSubtrees()
        {

        }
    }
}
