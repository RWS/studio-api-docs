# Writing style guide 
This topic aims to give you a good understanding about how to leverage language, structure sentences and documents to convey ideas or concepts in the best way possible.

## Active voice vs passive voice
In an active voice sentence, an actor acts on a target. That is, an active voice sentence follows this formula:
> Active Voice Sentence = actor + verb + target

A passive voice sentence reverses the formula. That is, a passive voice sentence typically follows the following formula:
> Passive Voice Sentence = target + verb + actor

Here's a short example for active voice:
> The cat sat on the mat.

* actor: The cat
* verd: sat 
* target: the mat

Here's a short example for passive voice: 
> The mat was sat on by the cat

* target: The mat
* passive verb: was sat
* actor: the cat

Some passive voice sentences omit an actor. For example:
> The mat was sat on.

* actor: unknown
* passive verb: was sat
* target: the mat

### Prefer active voice to passive voice

Use the active voice most of the time. Use the passive voice sparingly. Active voice provides the following advantages:

* Most readers mentally convert passive voice to active voice. Why subject your readers to extra processing time? By sticking to active voice, readers can skip the preprocessor stage and go straight to compilation.
* Passive voice obfuscates your ideas, turning sentences on their head. Passive voice reports action indirectly.
* Some passive voice sentences omit an actor altogether, which forces the reader to guess the actor's identity.
* Active voice is generally shorter than passive voice.

## Clear sentences
Many technical writers believe that the verb is the most important part of a sentence. Pick the right verb and the rest of the sentence will take care of itself. Unfortunately, some writers reuse only a small set of mild verbs, which is like serving your guests stale crackers and soggy lettuce every day. Picking the right verb takes a little more time but produces more satisfying results.

To engage and educate readers, choose precise, strong, specific verbs. Reduce imprecise, weak, or generic verbs, such as the following:

* forms of be: is, are, am, was, were, etc.
* occur
* happen

For example, consider how strengthening the weak verb in the following sentences ignites a more engaging sentence:

| Weak Verb | Strong Verb |
|-----------|-------------|
| The exception occurs when dividing by zero. | Dividing by zero **raises** the exception. |
| This error message happens when... | 	The system **generates** this error message when... |
| We are very careful to ensure... | We carefully **ensure**... |

<br/>
Many writers rely on forms of be as if they were the only spices on the rack. Sprinkle in different verbs and watch your prose become more appetizing. That said, a form of be is sometimes the best choice of verb, so don't feel that you have to eliminate every form of be from your writing.

Note that generic verbs often signal other ailments, such as:

* an imprecise or missing actor in a sentence
* a passive voice sentence

## Short sentences
Software engineers generally try to minimize the number of lines of code in an implementation for the following reasons:

* Shorter code is typically easier for others to read.
* Shorter code is typically easier to maintain than longer code.
* Extra lines of code introduce additional points of failure.

In fact, the same rules apply to technical writing:

* Shorter documentation reads faster than longer documentation.
* Shorter documentation is typically easier to maintain than longer documentation.
* Extra lines of documentation introduce additional points of failure.

Finding the shortest documentation implementation takes time but is ultimately worthwhile. Short sentences communicate more powerfully than long sentences, and short sentences are usually easier to understand than long sentences.

### Focus each sentence on a single idea
Focus each sentence on a single idea, thought, or concept. Just as statements in a program execute a single task, sentences should execute a single idea. For example, the following very long sentence contains multiple thoughts:

> The late 1950s was a key era for programming languages because IBM introduced Fortran in 1957 and John McCarthy introduced Lisp the following year, which gave programmers both an iterative way of solving problems and a recursive way.

Breaking the long sentence into a succession of single-idea sentences yields the following result:

> The late 1950s was a key era for programming languages. IBM introduced Fortran in 1957. John McCarthy invented Lisp the following year. Consequently, by the late 1950s, programmers could solve problems iteratively or recursively.