# PennyWiZe - finanza personale

Vorrei costruire un'app di finanza personale al fine di tracciare le mie entrate e uscite e i diversi portafogli che compongono il mio patrimonio liquido.

L'applicazione ruota attorno a due concetti principali: [_bilancio (conto economico)_](#bilancio-bilancio-conto-economico) e [_patrimonio (conto finanziario)_](#patrimonio-conto-finanziario).

## Bilancio bilancio (conto economico)

Il _bilancio_ è la cronologia di tutte le _transazioni_.  
Una _transazione_ è un movimento monetario in _entrata_ o _uscita_.  
Un _saldo_ è la somma di un insieme di _transazioni_.

## Patrimonio (conto finanziario)

Il _patrimonio_ è la somma della liquidità di tutti i _portafogli_.
Un _portafoglio_ è un particolare tipo di _saldo_ che rappresenta un _credito_ o un _debito_.

## Uso

Inizierò costruendo lo strumento da riga di comando `pwz`.

```
pwz transaction ls
pwz transaction add <new_transaction>
pwz transaction rm <transaction_id>
pwz transaction edit <transaction_id> <updated_transaction>
```
