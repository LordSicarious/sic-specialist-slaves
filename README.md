# [SIC] Specialist Slaves
A simple RimWorld mod that allows some ideology roles to be held by slaves.

From the base game, the Animals, Mining, Plants and Production Specialist roles can be slaves. Leaders, Moral Guides, and Combat or Research Specialists cannot be slaves. Each roles' work restrictions are applied on top of the natural restrictions from slavery, so some roles might be better held by free pawns to avoid locking out too many types of work!

In addition, this implements a simple framework that allows mods implementing other ideology roles to declare the slave status of each ideology role, as either "Forbidden", "Allowed", or even "Required"! Simply add the following code to your role's PreceptDef:

\<modExtensions>
  \<li Class="SpecialistSlaves.SpecialistSlaves_ModExtension">
    \<slavePermitted>**Forbidden/Allowed/Required**\</slavePermitted>
  \</li>
\</modExtensions>

Slaves are forbidden from holding roles by default if unspecified.
