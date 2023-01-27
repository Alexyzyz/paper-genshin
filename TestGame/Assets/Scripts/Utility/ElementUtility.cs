using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementUtility : MonoBehaviour
{

    public static ElementUtility Instance;

    [SerializeField]
    private List<Sprite> element_sprite_list;
    [SerializeField]
    private List<Color> element_color_list;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public Sprite GetElementSprite(GameData.Element element) => element_sprite_list[(int)element];

    public Color GetElementColor(GameData.Element element) => element_color_list[(int)element];

    public GameData.Reaction GetReaction(GameData.Element a, GameData.Element b) {
        Dictionary<Tuple<GameData.Element, GameData.Element>, GameData.Reaction> reaction_database = new() {

            { CreateReactionTuple(GameData.Element.PYRO, GameData.Element.HYDRO), GameData.Reaction.VAPORIZE },
            { CreateReactionTuple(GameData.Element.PYRO, GameData.Element.CRYO), GameData.Reaction.MELT },

            { CreateReactionTuple(GameData.Element.HYDRO, GameData.Element.CRYO), GameData.Reaction.FROZEN },

            { CreateReactionTuple(GameData.Element.PYRO, GameData.Element.ANEMO), GameData.Reaction.SWIRL },
            { CreateReactionTuple(GameData.Element.HYDRO, GameData.Element.ANEMO), GameData.Reaction.SWIRL },
            { CreateReactionTuple(GameData.Element.CRYO, GameData.Element.ANEMO), GameData.Reaction.SWIRL },

        };

        Tuple<GameData.Element, GameData.Element> elements;
        GameData.Reaction reaction;

        elements = CreateReactionTuple(a, b);
        if (reaction_database.TryGetValue(elements, out reaction)) return reaction;
        
        elements = CreateReactionTuple(b, a);
        if (reaction_database.TryGetValue(elements, out reaction)) return reaction;

        return GameData.Reaction.NONE;

        Tuple<GameData.Element, GameData.Element> CreateReactionTuple(GameData.Element a, GameData.Element b) => new(a, b);
    }

    public string GetReactionName(GameData.Reaction reaction) {
        Dictionary<GameData.Reaction, string> reaction_name_database = new() {
            { GameData.Reaction.NONE, "" },
            { GameData.Reaction.VAPORIZE, "Vaporize" },
            { GameData.Reaction.MELT, "Melt" },
            { GameData.Reaction.FROZEN, "Frozen" },
            { GameData.Reaction.SWIRL, "Swirl" },
        };
        return reaction_name_database.TryGetValue(reaction, out string reaction_name) ? reaction_name : "";
    }
    
}
