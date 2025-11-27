# Projet de Maintenance – Documentation Technique

## 1. Changements effectués et justification

### 🔧 1.1 Correctifs appliqués
- **Correction de la condition de transition entre les états "En Attente" et "En Exécution".**
  - *Justification :* une erreur logique empêchait la machine à états de lancer correctement le processus de maintenance.
  
- **Refactorisation du module de gestion des logs.**
  - *Justification :* amélioration de la lisibilité, homogénéité de structure et réduction de la duplication de code.

- **Ajout d’un mécanisme de timeout sur l’état "En Exécution".**
  - *Justification :* éviter le blocage permanent si aucune réponse n’était reçue du système supervisé.

### 🧩 1.2 Améliorations structurelles
- **Séparation du cœur de la machine à états et de la couche d’E/S.**
  - *Justification :* meilleure testabilité, modularité et maintenance future.
  
- **Introduction de tests unitaires pour chaque transition critique.**
  - *Justification :* réduire les régressions et garantir la fiabilité en production.


## 2. Diagramme de la machine à états

```mermaid
stateDiagram-v2
    [*] --> En_Attente

    En_Attente --> En_Execution : ordre_de_maintenance
    En_Execution --> En_Succès : opération_ok
    En_Execution --> En_Échec : erreur_detectée
    En_Execution --> Timeout : dépassement_temps
    Timeout --> En_Échec : annulation

    En_Succès --> En_Attente : reset
    En_Échec --> En_Attente : reset
