﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace dfa
{
    internal class DFA : Automaton {
        public Dictionary<(State, char), State> Transitions { get; set; }

        public DFA(NFA nfa)
        {
            States = new HashSet<State>();
            StartState = new State();
            FinalStates = new HashSet<State>();
            Transitions = new Dictionary<(State, char), State>();

            if (nfa.Alphabet != null)
            {
                Alphabet = nfa.Alphabet;
            }
        }

        private string getStatesString(int paddle) {
            string res = "";

            foreach (State state in States) {
                Dictionary<char, State> to_states = Transitions.Where(t => t.Key.Item1 == state).ToDictionary(t => t.Key.Item2, t => t.Value);

                string line = $"|{state.ToString().PadLeft(paddle)}|";

                foreach (char ch in Alphabet) {
                    if (to_states.ContainsKey(ch)) {
                        line += $"{to_states[ch].ToString().PadLeft(paddle)}|";
                    }
                    else {
                        line += $"{"".PadLeft(paddle)}|";
                    }
                }

                res += line + "\n";
            }

            return res;
        }

        public void PrintAutomaton()
        {
            bool check;
            string filePath = @"Automaton.txt";
            Console.WriteLine("Where do you want the automaton to be shown?\n1. Console\n2. File named Automaton.txt\n\nChoice: ");
            check = Convert.ToBoolean(Convert.ToInt16(Console.ReadLine())-1);
            if (check)
            {
                if (!File.Exists(filePath))
                {
                    FileStream f = File.Create(filePath);
                }
                StreamWriter sw = new StreamWriter(filePath);
                sw.WriteLine(this);
                sw.Close();
            }
            else
                Console.WriteLine(this);
        }

        public override string ToString()
        {
            int paddle = 4;
            string transitions_string = $"|{"δ".PadLeft(paddle)}|{"Σ".PadLeft((paddle+1)*Alphabet.Count - 1)}|\n";
            transitions_string += $"|{" ".PadLeft(paddle)}|{ string.Join("|", Alphabet.Select(ch => ch.ToString().PadLeft(paddle)))}|\n";
            transitions_string += getStatesString(paddle);

            //= string.Join("\n", Transitions.Select(t =>
            //$"({t.Key.Item1}, {t.Key.Item2?.ToString()}) -> {{ {string.Join(", ", t.Value)} }}"));

            return base.ToString() +
                "Delta: \n" +
                transitions_string +
                "\n";
        }

    }
}
