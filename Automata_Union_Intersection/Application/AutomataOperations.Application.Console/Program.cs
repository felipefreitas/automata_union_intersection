using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AutomataOperations.Application
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Count() == 0)
            {
                Console.WriteLine($"Usar: gerador [AFD's de entrada]");
                Environment.Exit(0);
            }

            #region PRE FILE PROCESSOR
            string filePath = args[0];
            List<string> programsToProcess = new List<string>();
            List<Automata> automatasToProcess = new List<Automata>();
            char[] alphabet;

            using (var file = new StreamReader(filePath))
            {
                string contentFile = "";
                contentFile = file.ReadToEnd();
                contentFile = Regex.Replace(contentFile, @"\n|\r|\s+", "");

                var separateByProgram = contentFile.Split(new[] { ";}" }, StringSplitOptions.RemoveEmptyEntries);

                var alphabetProgram = separateByProgram[0].Split(';')[0].Substring(1).Where(x => char.IsLetterOrDigit(x)).ToArray();
                alphabet = alphabetProgram;
                foreach (var item in separateByProgram)
                {
                    if (item.Split(';').Count() > 4)
                    {
                        programsToProcess.Add(item.Substring(separateByProgram[0].Split(';')[0].Length + 1) + '}');
                    }
                    else
                    {
                        programsToProcess.Add(item + '}');
                    }
                }
            }

            foreach (var program in programsToProcess)
            {
                var automata = StringToAutomata(program);
                automata.Alphabet = alphabet;
                automatasToProcess.Add(automata);
            } 
            #endregion

            #region CREATE AUTOMATA RESULT
            Automata automataResult = new Automata();
            automataResult.Alphabet = alphabet;

            foreach (var automata in automatasToProcess)
            {
                if (automataResult.InitialNode == null)
                {
                    automataResult.InitialNode = automata.InitialNode;
                }
                else
                {
                    automataResult.InitialNode = automataResult.InitialNode + '|' + automata.InitialNode;
                }
            }

            var toContinueProcess = true;
            var node = "";

            while (toContinueProcess)
            {
                foreach (var simbol in automataResult.Alphabet)
                {
                    string stateToBuild = "";
                    string stateToBuildGoes = "";

                    if (!string.IsNullOrWhiteSpace(node))
                    {
                        var nodeToProcess = node.Split('|');

                        for (int i = 0; i < nodeToProcess.Length; i++)
                        {
                            var next = "";
                            var automata = automatasToProcess[i];
                            var nodeAux = nodeToProcess[i];

                            if (string.IsNullOrWhiteSpace(nodeAux))
                            {
                                nodeAux = automata.InitialNode;
                            }

                            foreach (var transition in automata.Transitions)
                            {
                                if (transition.Item1 == nodeAux && transition.Item2 == simbol)
                                {
                                    next = transition.Item3;
                                    if (string.IsNullOrWhiteSpace(stateToBuildGoes))
                                    {
                                        stateToBuildGoes = next;
                                    }
                                    else
                                    {
                                        stateToBuildGoes = stateToBuildGoes + '|' + next;
                                    }
                                }
                            }

                            if (string.IsNullOrWhiteSpace(stateToBuildGoes))
                            {
                                stateToBuildGoes = "#";
                            }

                            if (string.IsNullOrWhiteSpace(stateToBuild))
                            {
                                stateToBuild = nodeAux;
                            }
                            else
                            {
                                stateToBuild = stateToBuild + '|' + nodeAux;
                            }
                        }
                    }
                    else
                    {
                        foreach (var automata in automatasToProcess)
                        {
                            var next = "";

                            node = automata.InitialNode;


                            foreach (var transition in automata.Transitions)
                            {
                                if (transition.Item1 == node && transition.Item2 == simbol)
                                {
                                    next = transition.Item3;
                                    if (string.IsNullOrWhiteSpace(stateToBuildGoes))
                                    {
                                        stateToBuildGoes = next;
                                    }
                                    else
                                    {
                                        stateToBuildGoes = stateToBuildGoes + '|' + next;
                                    }
                                }
                            }

                            if (string.IsNullOrWhiteSpace(stateToBuildGoes))
                            {
                                stateToBuildGoes = "#";
                            }

                            if (string.IsNullOrWhiteSpace(stateToBuild))
                            {
                                stateToBuild = node;
                            }
                            else
                            {
                                stateToBuild = stateToBuild + '|' + node;
                            }
                        }
                        node = "";
                    }

                    var transitionTuple = new Tuple<string, char, string>(stateToBuild, simbol, stateToBuildGoes);
                    if (!automataResult.Transitions.Contains(transitionTuple))
                    {
                        automataResult.Transitions.Add(transitionTuple);
                    }
                }

                foreach (var transition in automataResult.Transitions)
                {
                    if (!automataResult.States.Contains(transition.Item3))
                    {
                        automataResult.States.Add(transition.Item3);
                        node = transition.Item3;
                        toContinueProcess = true;
                        break;
                    }
                    node = "";
                    toContinueProcess = false;
                }
            }

            if (!automataResult.States.Contains(automataResult.InitialNode))
            {
                automataResult.States.Add(automataResult.InitialNode);
            }

            for (int i = 0; i < automatasToProcess.Count; i++)
            {
                SaveAutomataToDotFile(automatasToProcess[i], automatasToProcess[i].Name, false);
            }

            #endregion

            #region UNION PROCESS
            var automataToUnion = new Automata()
            {
                Name = "uniao",
                Alphabet = automataResult.Alphabet,
                Transitions = new List<Tuple<string, char, string>>(automataResult.Transitions),
                States = new List<string>(automataResult.States),
                FinalNodes = new List<string>(automataResult.FinalNodes),
                InitialNode = automataResult.InitialNode
            };

            foreach (var state in automataToUnion.States)
            {
                foreach (var automata in automatasToProcess)
                {
                    foreach (var finalState in automata.FinalNodes)
                    {
                        var states = state.Split('|');
                        foreach (var s in states)
                        {
                            if (s == finalState)
                            {
                                if (!automataToUnion.FinalNodes.Contains(state))
                                {
                                    automataToUnion.FinalNodes.Add(state);
                                }
                            }
                        }
                    }
                }
            }

            SaveAutomataToDotFile(automataToUnion, automataToUnion.Name, true);
            #endregion

            #region INTERSECTION PROCESS
            var automataToIntersection = new Automata()
            {
                Name = "intersecao",
                Alphabet = automataResult.Alphabet,
                Transitions = new List<Tuple<string, char, string>>(automataResult.Transitions),
                States = new List<string>(automataResult.States),
                FinalNodes = new List<string>(automataResult.FinalNodes),
                InitialNode = automataResult.InitialNode
            };
            
            List<string> finalStatesFromAll = new List<string>();

            foreach (var automata in automatasToProcess)
            {
                foreach (var final in automata.FinalNodes)
                {
                    finalStatesFromAll.Add(final);
                }
            }

            foreach (var state in automataToIntersection.States)
            {
                var subStates = state.Split('|');
                var isFinalState = true;

                for (int i = 0; i < subStates.Length; i++)
                {
                    if(finalStatesFromAll[i] != subStates[i])
                    {
                        isFinalState = false;
                    }
                }

                if (isFinalState)
                {
                    automataToIntersection.FinalNodes.Add(state);
                }
            }

            SaveAutomataToDotFile(automataToIntersection, automataToIntersection.Name, true); 
            #endregion
        }

        public class Automata
        {
            public char[] Alphabet { get; set; }
            public string InitialNode { get; set; }
            public List<string> States { get; set; }
            public List<Tuple<string, char, string>> Transitions { get; set; }
            public List<string> FinalNodes { get; set; }
            public string Name { get; set; }

            public Automata()
            {
                States = new List<string>();
                Transitions = new List<Tuple<string, char, string>>();
                FinalNodes = new List<string>();
            }
        }

        public enum ENextSteps
        {
            Undefined,
            Begin,
            States,
            Transitions,
            BuildTransitions,
            InitialState,
            FinalStates,
            ReadTransitions
        }

        public static Automata StringToAutomata(string program)
        {
            Stack<char> control = new Stack<char>();
            Stack<ENextSteps> sequenceStep = new Stack<ENextSteps>();

            sequenceStep.Push(ENextSteps.FinalStates);
            sequenceStep.Push(ENextSteps.InitialState);
            sequenceStep.Push(ENextSteps.BuildTransitions);
            sequenceStep.Push(ENextSteps.ReadTransitions);
            sequenceStep.Push(ENextSteps.Transitions);
            sequenceStep.Push(ENextSteps.States);
            sequenceStep.Push(ENextSteps.Begin);

            Automata automata = new Automata();
            ENextSteps toDo = ENextSteps.Undefined;
            string toBuild = "";
            Tuple<string, char, string> toBuilTuple = new Tuple<string, char, string>("", ' ', "");
            string nameBuilder = "";
            var iterateName = program.GetEnumerator();

            iterateName.MoveNext();
            while (iterateName.Current != '{')
            {
                nameBuilder = nameBuilder + iterateName.Current;
                iterateName.MoveNext();
            }

            automata.Name = nameBuilder;

            for (int i = 0; i < program.Length; i++)
            {
                var c = program[i];
                var cNext = '#';
                if (c == '{' || c == '}')
                {
                    if (sequenceStep.Count > 0)
                    {
                        toDo = sequenceStep.Pop();
                        cNext = program[i + 1];
                    }
                    else
                    {
                        toDo = ENextSteps.Undefined;
                    }
                }
                else
                {
                    cNext = program[i + 1];
                }

                switch (toDo)
                {
                    case ENextSteps.Begin:
                        {
                            break;
                        }
                    case ENextSteps.States:
                        {
                            if (char.IsLetterOrDigit(c))
                            {
                                toBuild = toBuild + c;
                            }
                            if (c == ',' || cNext == '}')
                            {
                                automata.States.Add(toBuild);
                                toBuild = "";
                            }
                        }
                        break;
                    case ENextSteps.ReadTransitions:
                        {
                            if (char.IsLetterOrDigit(c) && cNext != '-')
                            {
                                toBuild = toBuild + c;
                            }
                            if (c == ',' && cNext != '(' || c == '=')
                            {
                                toBuild = toBuild + '|';
                            }
                            if (toBuild.Split('|').Length == 3)
                            {
                                var itens = toBuild.Split('|');
                                if (automata.States.Contains(itens[0]) && automata.States.Contains(itens[2]))
                                {
                                    automata.Transitions.Add(new Tuple<string, char, string>(itens[0], itens[1].ToCharArray()[0], itens[2]));
                                    toBuild = "";
                                }
                            }
                            break;
                        }
                    case ENextSteps.BuildTransitions:
                        {
                            toDo = ENextSteps.InitialState;
                            break;
                        }
                    case ENextSteps.InitialState:
                        {
                            if (char.IsLetterOrDigit(c) && cNext != '-')
                            {
                                toBuild = toBuild + c;
                            }
                            if (automata.States.Contains(toBuild) && cNext == ';')
                            {
                                automata.InitialNode = toBuild;
                                toBuild = "";

                                toDo = ENextSteps.FinalStates;
                            }
                            break;
                        }
                    case ENextSteps.FinalStates:
                        {
                            if (char.IsLetterOrDigit(c) && cNext != '-')
                            {
                                toBuild = toBuild + c;
                            }
                            if (c == ',' || cNext == '}')
                            {
                                automata.FinalNodes.Add(toBuild);
                                toBuild = "";
                            }
                            break;
                        }
                    case ENextSteps.Undefined:
                        break;
                    default:
                        break;
                }
            }
            return automata;
        }

        public static void SaveAutomataToDotFile(Automata automata, string automataName, bool withBracker)
        {
            using (StreamWriter stream = new StreamWriter(automataName + ".dot"))
            {
                stream.WriteLine($"digraph \"{automataName}\" {{");
                stream.WriteLine($"_nil [style=\"invis\"];");

                if (withBracker)
                {
                    stream.WriteLine($"_nil -> \"[{automata.InitialNode.Replace('|', ',')}]\" [label=\"\"];");
                }
                else
                {
                    stream.WriteLine($"_nil -> \"{automata.InitialNode.Replace('|', ',')}\" [label=\"\"];");
                }

                foreach (var finalState in automata.FinalNodes)
                {
                    if (withBracker)
                    {
                        stream.WriteLine($"\"[{finalState.Replace('|', ',').Replace("#", string.Empty)}]\" [peripheries=2];");
                    }
                    else
                    {
                        stream.WriteLine($"\"{finalState.Replace('|', ',').Replace("#", string.Empty)}\" [peripheries=2];");
                    }
                }

                foreach (var transition in automata.Transitions)
                {
                    if (withBracker)
                    {
                        stream.WriteLine($"\"[{transition.Item1.Replace('|', ',').Replace("#", string.Empty)}]\" -> \"[{transition.Item3.Replace('|', ',').Replace("#", string.Empty)}]\" [label={transition.Item2}];");
                    }
                    else
                    {
                        stream.WriteLine($"\"{transition.Item1.Replace('|', ',').Replace("#", string.Empty)}\" -> \"{transition.Item3.Replace('|', ',').Replace("#", string.Empty)}\" [label={transition.Item2}];");
                    }
                }
                stream.WriteLine(String.Format("{0}", "}"));
            }
        }
    }
}
