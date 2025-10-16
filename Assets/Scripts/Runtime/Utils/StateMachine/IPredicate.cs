using System.Collections.Generic;
using System.Linq;
using Alchemy.Inspector;
using UnityEngine;
using ZLinq;

namespace ElusiveLife.Utils.Assets.Scripts.Runtime.Utils.StateMachine
{
    public interface IPredicate
    {
        bool Evaluate();
    }

    public class And : IPredicate
    {
        [SerializeField] private readonly List<IPredicate> _rules = new();

        public bool Evaluate() => _rules.AsValueEnumerable().All(r => r.Evaluate());
    }

    public class Or : IPredicate
    {
        [SerializeField] private readonly List<IPredicate> _rules = new();

        public bool Evaluate() => _rules.AsValueEnumerable().Any(r => r.Evaluate());
    }

    public class Not : IPredicate
    {
        [SerializeField, LabelWidth(80f)] private IPredicate _rules;

        public bool Evaluate() => !_rules.Evaluate();
    }
}